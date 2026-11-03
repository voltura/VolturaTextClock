using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using VolturaTextClock;
using VolturaTextClock.Updates;

var scratch = Path.Combine(Path.GetTempPath(), "TextClock-check-" + Guid.NewGuid().ToString("N"));
Directory.CreateDirectory(scratch);
try
{
    var settings = Path.Combine(scratch, "settings.json");
    File.WriteAllText(settings, "{\"appSettings\":{\"clockTheme\":\"{custom}\",\"autoStart\":true,\"futureSetting\":42}}");
    SettingsStore.Initialize(settings);
    var saved = SettingsStore.Read(settings);
    Check(saved["appSettings"]!["clockTheme"]!.ToString() == "{custom}" && (bool)saved["appSettings"]!["autoStart"]!, "migration preserves old settings");
    SettingsStore.Save(settings, new Dictionary<string, object> { ["interfaceLanguage"] = "Swedish" });
    Check((int)SettingsStore.Read(settings)["appSettings"]!["futureSetting"]! == 42, "unknown settings survive save");
    File.WriteAllText(settings, "{broken");
    SettingsStore.Initialize(settings);
    Check(Directory.GetFiles(scratch, "settings.json.corrupt-*").Length == 1 && !(bool)SettingsStore.Read(settings)["appSettings"]!["autoStart"]!, "malformed settings backed up with safe defaults");
    SettingsStore.Save(settings, new Dictionary<string, object> { ["autoStart"] = "invalid", ["logFileSizeMB"] = "99999999999999999" });
    var config = new ConfigurationBuilder().AddJsonFile(settings).Build();
    using ((IDisposable)config)
    {
        Check(!ConfigExtensions.GetValue(config, "autoStart", false), "invalid boolean falls back");
        Check(ConfigExtensions.GetValue(config, "logFileSizeMB", 10) == 10, "overflow falls back");
    }
    Check(Directory.GetFiles(scratch, "*.tmp").Length == 0, "atomic settings leave no partial file");
    UiText.Language = "Swedish"; Check(UiText.Get("Save") == "Spara", "Swedish UI");
    UiText.Language = "English"; Check(UiText.Get("Save") == "Save", "English UI independent of clock");

    using var rsa = RSA.Create(2048);
    var payload = Encoding.UTF8.GetBytes("test package; never executed");
    var version = "1.2.0";
    var name = $"VolturaTextClock-Setup-{version}-win-x64.exe";
    byte[] Manifest(string v, string assetName, long size, string hash) => JsonSerializer.SerializeToUtf8Bytes(new { schema = 1, version = v, assets = new[] { new { name = assetName, size, sha256 = hash } } });
    var manifest = Manifest(version, name, payload.Length, Convert.ToHexString(SHA256.HashData(payload)));
    byte[] Sign(byte[] data) => rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pss);
    var signature = Sign(manifest);
    var verified = UpdateVerifier.Verify(manifest, signature, rsa.ExportSubjectPublicKeyInfoPem(), false);
    Check(verified.Asset.Name == name, "signed manifest accepted");
    Reject(() => UpdateVerifier.Verify(manifest, new byte[256], rsa.ExportSubjectPublicKeyInfoPem(), false), "invalid signature rejected");
    var badName = Manifest(version, "../other.exe", payload.Length, new string('a', 64));
    Reject(() => UpdateVerifier.Verify(badName, Sign(badName), rsa.ExportSubjectPublicKeyInfoPem(), false), "unexpected asset rejected");
    var huge = Manifest(version, name, UpdateVerifier.MaximumInstallerBytes + 1L, new string('a', 64));
    Reject(() => UpdateVerifier.Verify(huge, Sign(huge), rsa.ExportSubjectPublicKeyInfoPem(), false), "oversized installer rejected");

    byte[] Release(string tag = "v1.2.0", bool evil = false) => JsonSerializer.SerializeToUtf8Bytes(new
    {
        tag_name = tag, draft = false, prerelease = false,
        assets = new[] { name, "VolturaTextClock-Update-1.2.0.json", "VolturaTextClock-Update-1.2.0.sig" }
            .Select(asset => new { name = asset, browser_download_url = (evil ? "https://example.com/" : "https://github.com/voltura/VolturaTextClock/releases/download/v1.2.0/") + asset })
    });
    var installed = false;
    var handler = new FakeHandler(uri => uri.Host == "api.github.com" ? Release() : uri.AbsolutePath.EndsWith(".json") ? manifest : uri.AbsolutePath.EndsWith(".sig") ? signature : payload);
    await using (var service = new UpdateService(Path.Combine(scratch, "good"), handler, rsa.ExportSubjectPublicKeyInfoPem(), true, false, new Version(1, 1, 0), install: (_, _) => { installed = true; return Task.CompletedTask; }))
    {
        await service.CheckAsync();
        Check(service.State.Status == UpdateStatus.Available && handler.InstallerRequests == 0, "check does not download installer");
        await service.CheckAsync(download: true);
        Check(service.State.Ready, "download verified and cached");
        File.AppendAllText(service.State.Installer!, "tampered");
        Check(!await service.InstallAsync() && !installed, "tampered cached installer never launches");
        await service.CheckAsync(download: true);
        Check(service.State.Ready && await service.InstallAsync() && installed, "retry restores valid update and hands off once");
    }
    await using (var service = new UpdateService(Path.Combine(scratch, "old"), new FakeHandler(_ => Release("v1.0.9")), rsa.ExportSubjectPublicKeyInfoPem(), true, false, new Version(1, 1, 0)))
    { await service.CheckAsync(true); Check(service.State.Status == UpdateStatus.Current, "older release is never installed"); }
    await using (var service = new UpdateService(Path.Combine(scratch, "origin"), new FakeHandler(_ => Release(evil: true)), rsa.ExportSubjectPublicKeyInfoPem(), true, false, new Version(1, 1, 0)))
    { await service.CheckAsync(true); Check(!service.State.Ready && service.State.Status == UpdateStatus.UpdateDownloadFailed, "foreign release URL rejected"); }
    var stalled = new FakeHandler(_ => payload) { Stall = true };
    await using (var service = new UpdateService(Path.Combine(scratch, "cancel"), stalled, rsa.ExportSubjectPublicKeyInfoPem(), true, false))
    {
        var operation = service.CheckAsync(true);
        await stalled.Started.Task.WaitAsync(TimeSpan.FromSeconds(5));
        service.Cancel(); await operation;
        Check(service.State.Status == UpdateStatus.Cancelled && !service.State.Ready, "cancelled network operation leaves no runnable update");
    }
    var interrupted = new FakeHandler(_ => throw new HttpRequestException("connection interrupted"));
    await using (var service = new UpdateService(Path.Combine(scratch, "offline"), interrupted, rsa.ExportSubjectPublicKeyInfoPem(), true, false))
    { await service.CheckAsync(); Check(service.State.Status == UpdateStatus.UpdateCheckFailed, "network failure is recoverable UI state"); }
    Console.WriteLine("PASS settings recovery, independent UI languages, update trust boundaries, cancellation and retry");
}
finally { Directory.Delete(scratch, true); }

static void Check(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); Console.WriteLine("PASS " + message); }
static void Reject(Action action, string message)
{
    try { action(); } catch (Exception error) when (error is InvalidDataException or CryptographicException) { Console.WriteLine("PASS " + message); return; }
    throw new InvalidOperationException(message);
}
sealed class FakeHandler(Func<Uri, byte[]> content) : HttpMessageHandler
{
    public int InstallerRequests;
    public bool Stall;
    public TaskCompletionSource Started = new(TaskCreationOptions.RunContinuationsAsynchronously);
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken token)
    {
        Started.TrySetResult();
        if (Stall) await Task.Delay(Timeout.Infinite, token);
        if (request.RequestUri!.AbsolutePath.EndsWith(".exe")) InstallerRequests++;
        return new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(content(request.RequestUri)) };
    }
}
