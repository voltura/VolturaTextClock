#Requires -Version 7.0
param([ValidateSet('standard','full')][string]$Variant = 'full')
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
[xml]$metadata = Get-Content (Join-Path $root 'Directory.Build.props')
$version = [string]$metadata.Project.PropertyGroup.Version
$testRoot = Join-Path $root ('artifacts/sandbox-' + $Variant)
$inputPath = Join-Path $testRoot 'input'
$outputPath = Join-Path $testRoot 'output'
New-Item -ItemType Directory -Force $inputPath,$outputPath | Out-Null
Remove-Item -LiteralPath (Join-Path $outputPath 'result.json') -ErrorAction SilentlyContinue
$suffix = if ($Variant -eq 'full') { '-full' } else { '' }
Copy-Item -LiteralPath (Join-Path $root "artifacts/publish/VolturaTextClock-Setup-$version-win-x64$suffix.exe") -Destination (Join-Path $inputPath 'setup.exe') -Force
$script = @'
$ErrorActionPreference = 'Stop'
if ($env:USERNAME -ne 'WDAGUtilityAccount') { throw 'This acceptance script must run only inside Windows Sandbox.' }
$result = [ordered]@{ user = $env:USERNAME; computer = $env:COMPUTERNAME; passed = $false; checks = @() }
try {
    $settings = Join-Path $env:LOCALAPPDATA 'VolturaTextClockAppsettings.json'
    [IO.File]::WriteAllText($settings, '{"appSettings":{"autoStart":false,"automaticUpdateCheck":false,"clockTheme":"{\"ActiveColor\":\"#AADDFF\"}","mainFormLocation":"40,40"}}')
    $before = [IO.File]::ReadAllText($settings)
    $hadRuntime = Test-Path "$env:ProgramFiles\dotnet\shared\Microsoft.WindowsDesktop.App"
    $result.hadDesktopRuntime = $hadRuntime
    $setup = Start-Process -FilePath 'C:\TextClockInput\setup.exe' -ArgumentList '/S' -PassThru -WindowStyle Hidden
    if (-not $setup.WaitForExit(600000) -or $setup.ExitCode -notin @(0,3010)) { throw 'Fresh setup failed or timed out.' }
    $result.checks += 'Fresh setup completed'
    $target = Join-Path $env:LOCALAPPDATA 'Programs\VolturaTextClock'
    $key = Get-ItemProperty 'HKCU:\Software\Microsoft\Windows\CurrentVersion\Uninstall\VolturaTextClock'
    if ($key.DisplayName -ne 'Voltura TextClock' -or $key.Publisher -ne 'Voltura AB' -or $key.InstallLocation -ne $target) { throw 'Installed Apps metadata incorrect.' }
    if ([IO.File]::ReadAllText($settings) -ne $before) { throw 'Installer changed old portable settings.' }
    $result.checks += 'Installed Apps metadata and legacy portable settings retained'
    $health = Start-Process -FilePath (Join-Path $target 'VolturaTextClock.exe') -ArgumentList '--installer-health-check' -PassThru -WindowStyle Hidden
    if (-not $health.WaitForExit(30000) -or $health.ExitCode -ne 0) { throw 'Installed app health check failed.' }
    $result.checks += 'Installed app starts with its packaged/runtime prerequisites'
    $again = Start-Process -FilePath 'C:\TextClockInput\setup.exe' -ArgumentList '/S' -PassThru -WindowStyle Hidden
    if (-not $again.WaitForExit(300000) -or $again.ExitCode -ne 0) { throw 'Reinstall/upgrade failed.' }
    if ([IO.File]::ReadAllText($settings) -ne $before) { throw 'Reinstall changed existing settings.' }
    $result.checks += 'Reinstall/upgrade retained settings'
    $uninstall = Start-Process -FilePath (Join-Path $target 'Uninstall.exe') -ArgumentList '/S' -PassThru -WindowStyle Hidden
    if (-not $uninstall.WaitForExit(120000)) { throw 'Uninstall timed out.' }
    $result.uninstallLauncherExit = $uninstall.ExitCode
    $deadline = [DateTime]::UtcNow.AddSeconds(180)
    $base = Split-Path -Parent $target
    $journal = Join-Path $base '.VolturaTextClock-maintenance\journal.json'
    while (((Test-Path -LiteralPath $target) -or (Test-Path -LiteralPath $journal) -or (Test-Path 'HKCU:\Software\Microsoft\Windows\CurrentVersion\Uninstall\VolturaTextClock')) -and [DateTime]::UtcNow -lt $deadline) { Start-Sleep -Milliseconds 500 }
    if ((Test-Path -LiteralPath $target) -or (Test-Path -LiteralPath $journal) -or (Test-Path 'HKCU:\Software\Microsoft\Windows\CurrentVersion\Uninstall\VolturaTextClock')) { throw 'Uninstall did not finish.' }
    if ([IO.File]::ReadAllText($settings) -ne $before) { throw 'Uninstall removed user settings.' }
    $result.checks += 'Uninstall removed owned installation and retained settings'

    # Replay an interrupted uninstall after its files and startup value are gone.
    $pendingRemoval = @{
        Mode = 'Uninstall'
        Phase = 'Removing'
        Stage = Join-Path $base ('.VolturaTextClock-stage-' + [Guid]::NewGuid().ToString('N'))
        Backup = Join-Path $base ('.VolturaTextClock-backup-' + [Guid]::NewGuid().ToString('N'))
        HadPrevious = $true
        PreviousVariant = 'standard'
    }
    [IO.File]::WriteAllText($journal, ($pendingRemoval | ConvertTo-Json))
    $recovery = Start-Process -FilePath 'C:\TextClockInput\setup.exe' -ArgumentList '/S' -PassThru -WindowStyle Hidden
    if (-not $recovery.WaitForExit(300000) -or $recovery.ExitCode -ne 0 -or
        (Test-Path -LiteralPath $journal) -or -not (Test-Path -LiteralPath (Join-Path $target 'VolturaTextClock.exe'))) {
        throw 'Setup did not recover the interrupted uninstall.'
    }
    $result.checks += 'Setup recovered an interrupted uninstall with no startup entry'
    $result.passed = $true
} catch {
    $result.error = $_.Exception.ToString()
    $result.remainingProcesses = @(Get-Process -ErrorAction SilentlyContinue | Where-Object { $_.ProcessName -match 'TextClock|Uninstall|Au_|powershell' } | Select-Object ProcessName,Id)
    $result.remainingFiles = @(Get-ChildItem -LiteralPath (Join-Path $env:LOCALAPPDATA 'Programs') -Filter '*TextClock*' -Force -ErrorAction SilentlyContinue | Select-Object Name)
    $journal = Join-Path $env:LOCALAPPDATA 'Programs\.VolturaTextClock-maintenance\journal.json'
    if (Test-Path -LiteralPath $journal) { $result.maintenanceJournal = [IO.File]::ReadAllText($journal) }
}
finally {
    $result.finishedUtc = [DateTime]::UtcNow.ToString('O')
    [IO.File]::WriteAllText('C:\TextClockOutput\result.json', ($result | ConvertTo-Json -Depth 5))
    shutdown.exe /s /t 0
}
'@
[IO.File]::WriteAllText((Join-Path $inputPath 'acceptance.ps1'), $script)
$networking = if ($Variant -eq 'full') { 'Disable' } else { 'Enable' }
$inputXml = [Security.SecurityElement]::Escape($inputPath)
$outputXml = [Security.SecurityElement]::Escape($outputPath)
$configuration = @"
<Configuration>
  <Networking>$networking</Networking>
  <ClipboardRedirection>Disable</ClipboardRedirection>
  <MappedFolders>
    <MappedFolder><HostFolder>$inputXml</HostFolder><SandboxFolder>C:\TextClockInput</SandboxFolder><ReadOnly>true</ReadOnly></MappedFolder>
    <MappedFolder><HostFolder>$outputXml</HostFolder><SandboxFolder>C:\TextClockOutput</SandboxFolder><ReadOnly>false</ReadOnly></MappedFolder>
  </MappedFolders>
  <LogonCommand><Command>powershell.exe -NoProfile -ExecutionPolicy Bypass -File C:\TextClockInput\acceptance.ps1</Command></LogonCommand>
</Configuration>
"@
$wsb = Join-Path $testRoot 'acceptance.wsb'
[IO.File]::WriteAllText($wsb, $configuration)
$sandbox = Start-Process -FilePath 'WindowsSandbox.exe' -ArgumentList ('"' + $wsb + '"') -PassThru -WindowStyle Hidden
Write-Host "Started clean $Variant acceptance sandbox. Results: $outputPath/result.json (process $($sandbox.Id))"
