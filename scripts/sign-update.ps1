#Requires -Version 7.0
param([string]$KeyPath = (Join-Path $env:USERPROFILE '.voltura/signing/textclock-update.dpapi'))
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$key = [IO.Path]::GetFullPath($KeyPath)
if ($key.StartsWith($root + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase)) { throw 'Private key must remain outside the repository.' }
Add-Type -AssemblyName System.Security
$private = [Security.Cryptography.ProtectedData]::Unprotect([IO.File]::ReadAllBytes($key), $null, [Security.Cryptography.DataProtectionScope]::CurrentUser)
$rsa = [Security.Cryptography.RSA]::Create()
try {
    $read = 0
    $rsa.ImportPkcs8PrivateKey($private, [ref]$read)
    $public = [IO.File]::ReadAllText((Join-Path $root 'VolturaTextClock/Updates/update-signing-public.pem'))
    if ($rsa.ExportSubjectPublicKeyInfoPem().Trim() -cne $public.Trim()) { throw 'Signing key does not match the embedded public key.' }
    [xml]$metadata = Get-Content (Join-Path $root 'Directory.Build.props')
    $version = [string]$metadata.Project.PropertyGroup.Version
    $publish = Join-Path $root 'artifacts/publish'
    $assets = @('', '-full') | ForEach-Object {
        $file = Get-Item -LiteralPath (Join-Path $publish "VolturaTextClock-Setup-$version-win-x64$_.exe")
        [ordered]@{ name = $file.Name; size = $file.Length; sha256 = (Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash.ToLowerInvariant() }
    }
    $manifest = [Text.Encoding]::UTF8.GetBytes((ConvertTo-Json -InputObject ([ordered]@{ schema = 1; version = $version; assets = @($assets) }) -Depth 5 -Compress))
    $signature = $rsa.SignData($manifest, [Security.Cryptography.HashAlgorithmName]::SHA256, [Security.Cryptography.RSASignaturePadding]::Pss)
    [IO.File]::WriteAllBytes((Join-Path $publish "VolturaTextClock-Update-$version.json"), $manifest)
    [IO.File]::WriteAllBytes((Join-Path $publish "VolturaTextClock-Update-$version.sig"), $signature)
} finally { [Array]::Clear($private, 0, $private.Length); $rsa.Dispose() }
