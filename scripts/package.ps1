#Requires -Version 7.0
param([switch]$SkipChecks, [string]$SigningConfig = (Join-Path $PSScriptRoot 'signing.json'), [switch]$Unsigned)
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
Push-Location $root
try {
    [xml]$metadata = Get-Content Directory.Build.props
    $version = [string]$metadata.Project.PropertyGroup.Version
    if ($version -cnotmatch '^\d+\.\d+\.\d+$') { throw 'Use a stable three-part version.' }
    $nsis = Join-Path ${env:ProgramFiles(x86)} 'NSIS/makensis.exe'
    if (-not (Test-Path -LiteralPath $nsis)) { throw 'NSIS is required.' }
    if (-not $Unsigned) {
        $signing = Get-Content -LiteralPath $SigningConfig -Raw | ConvertFrom-Json
        $signingArguments = @{
            CertificateThumbprint = $signing.certificateThumbprint
            TimestampUrl = $signing.timestampUrl
            AllowSelfSigned = $signing.allowSelfSigned -eq $true
        }
        & "$PSScriptRoot/sign.ps1" -Path 'preflight.exe' -ValidateOnly @signingArguments
    }
    function Sign-Executable([string]$path) {
        if (-not $Unsigned) {
            & "$PSScriptRoot/sign.ps1" -Path $path @signingArguments
        }
    }
    function Reset-Payload([string]$name) {
        if ($name -notin @('payload-standard','payload-full','portable')) { throw 'Unexpected build directory.' }
        $parent = [IO.Path]::GetFullPath((Join-Path $root 'artifacts'))
        $path = [IO.Path]::GetFullPath((Join-Path $parent $name))
        if ([IO.Path]::GetDirectoryName($path) -ne $parent) { throw 'Build path escaped artifacts.' }
        if (Test-Path -LiteralPath $path) {
            $linked = (Get-Item -LiteralPath $path).Attributes -band [IO.FileAttributes]::ReparsePoint
            if ($linked -or (Get-ChildItem -LiteralPath $path -Recurse -Force -Attributes ReparsePoint)) { throw 'Linked build directories are not supported.' }
            Remove-Item -LiteralPath $path -Recurse -Force
        }
        New-Item -ItemType Directory -Path $path -Force | Out-Null
        return $path
    }
    if (-not $SkipChecks) { & "$PSScriptRoot/verify.ps1" }
    & "$PSScriptRoot/build-branding.ps1"
    $publish = Join-Path $root 'artifacts/publish'
    New-Item -ItemType Directory -Force $publish | Out-Null
    foreach ($variant in @('standard','full')) {
        $payload = Reset-Payload "payload-$variant"
        dotnet publish VolturaTextClock/VolturaTextClock.csproj -c Release -r win-x64 --self-contained ($variant -eq 'full').ToString().ToLowerInvariant() -o $payload -p:DebugType=None -p:DebugSymbols=false
        if ($LASTEXITCODE) { throw 'Publish failed.' }
        Copy-Item -LiteralPath LICENSE,THIRD-PARTY-NOTICES.md,docs/earner-MIT-LICENSE.txt -Destination $payload
        if ($variant -eq 'full') {
            $runtime = Get-Content (Join-Path $payload 'VolturaTextClock.runtimeconfig.json') -Raw | ConvertFrom-Json
            $nuget = if ($env:NUGET_PACKAGES) { $env:NUGET_PACKAGES } else { Join-Path $env:USERPROFILE '.nuget/packages' }
            foreach ($framework in $runtime.runtimeOptions.includedFrameworks) {
                $package = Join-Path $nuget ($framework.name.ToLowerInvariant() + '.runtime.win-x64/' + $framework.version)
                $notices = Get-ChildItem -LiteralPath $package -File | Where-Object { $_.Name -like 'LICENSE*' -or $_.Name -like '*NOTICE*' }
                if (-not $notices) { throw "Runtime notices missing for $($framework.name)." }
                $destination = Join-Path $payload ('ThirdPartyNotices/' + $framework.name)
                New-Item -ItemType Directory -Force $destination | Out-Null
                $notices | Copy-Item -Destination $destination
            }
        }
        & "$PSScriptRoot/set-exe-metadata.ps1" -Path (Join-Path $payload 'VolturaTextClock.exe') -Version $version
        Sign-Executable (Join-Path $payload 'VolturaTextClock.exe')
        $entries = @(Get-ChildItem -LiteralPath $payload -File -Recurse | ForEach-Object {
            @{ path = $_.FullName.Substring($payload.Length + 1); size = $_.Length; sha256 = (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash }
        })
        [IO.File]::WriteAllText((Join-Path $payload 'payload.json'), (ConvertTo-Json -InputObject $entries -Depth 4))
        $suffix = if ($variant -eq 'full') { '-full' } else { '' }
        $output = Join-Path $publish "VolturaTextClock-Setup-$version-win-x64$suffix.exe"
        & $nsis /WX "/DVERSION=$version" "/DVARIANT=$variant" "/DPAYLOAD=$payload" "/DOUTPUT=$output" installer/VolturaTextClock.nsi
        if ($LASTEXITCODE) { throw 'NSIS compilation failed.' }
        Sign-Executable $output
    }
    $portable = Reset-Payload 'portable'
    Get-ChildItem artifacts/payload-full | Where-Object Name -ne 'payload.json' | Copy-Item -Destination $portable -Recurse
    [IO.File]::WriteAllText((Join-Path $portable 'portable.marker'), 'VolturaTextClock.Portable.v1')
    Compress-Archive -Path "$portable/*" -DestinationPath (Join-Path $publish "VolturaTextClock-$version-win-x64.zip") -Force
    if (-not $Unsigned) { & "$PSScriptRoot/sign-update.ps1" }
    $prepared = [ordered]@{
        revision = (git rev-parse HEAD)
        dirty = [bool](git status --porcelain)
        version = $version
        signed = -not $Unsigned
        assets = @(Get-ChildItem -LiteralPath $publish -File | Where-Object { $_.Name -match "^VolturaTextClock-(Setup-|Update-)?$([regex]::Escape($version))" } | ForEach-Object {
            @{ name = $_.Name; sha256 = (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash }
        })
    }
    [IO.File]::WriteAllText((Join-Path $root 'artifacts/preparation.json'), ($prepared | ConvertTo-Json -Depth 4))
    Write-Host "Prepared Voltura TextClock $version in $publish"
} finally { Pop-Location }
