#Requires -Version 7.0
param([switch]$PrepareOnly, [switch]$Publish, [switch]$UsePrepared)
$ErrorActionPreference = 'Stop'
if ($PrepareOnly -and $Publish) { throw 'Choose preparation or publication.' }
$root = Split-Path $PSScriptRoot -Parent
$repository = 'voltura/VolturaTextClock'
Push-Location $root
try {
    [xml]$metadata = Get-Content Directory.Build.props
    $version = [string]$metadata.Project.PropertyGroup.Version
    if ($version -cnotmatch '^(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)$') { throw 'Invalid stable version.' }
    $revision = git rev-parse HEAD
    if ($LASTEXITCODE) { throw 'Cannot identify source commit.' }
    if ($Publish) {
        if ((git branch --show-current) -ne 'master' -or (git status --porcelain)) { throw 'Publish only clean reviewed master.' }
        $remote = git ls-remote origin refs/heads/master
        if ($LASTEXITCODE -or ($remote -split '\s+')[0] -ne $revision) { throw 'Local master must match origin/master.' }
        $tag = git ls-remote origin "refs/tags/v$version"
        if ($LASTEXITCODE -or $tag) { throw 'Release tag already exists or remote lookup failed.' }
        $runs = gh run list --repo $repository --commit $revision --json workflowName,conclusion,status --limit 20 | ConvertFrom-Json
        if ($LASTEXITCODE) { throw 'Cannot read CI checks.' }
        foreach ($workflow in @('Build and validate clock','CodeQL')) {
            $run = @($runs | Where-Object workflowName -eq $workflow | Select-Object -First 1)
            if ($run.Count -ne 1 -or $run[0].status -ne 'completed' -or $run[0].conclusion -ne 'success') { throw "$workflow must succeed for this exact commit before release." }
        }
    }
    $notes = Join-Path $root "docs/releases/$version.md"
    if (-not (Test-Path -LiteralPath $notes)) { throw 'Release notes are required.' }
    if ($UsePrepared) {
        $prepared = Get-Content artifacts/preparation.json -Raw | ConvertFrom-Json
        if ($prepared.revision -ne $revision -or $prepared.version -ne $version -or $prepared.dirty -or -not $prepared.signed -or @($prepared.assets).Count -ne 5) { throw 'Prepared artifacts do not match clean signed source.' }
        foreach ($asset in $prepared.assets) {
            if ($asset.name -ne [IO.Path]::GetFileName($asset.name)) { throw 'Invalid prepared asset name.' }
            $path = Join-Path $root ('artifacts/publish/' + $asset.name)
            if ((Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash -ne $asset.sha256) { throw 'Prepared asset changed after verification.' }
        }
    } else { & "$PSScriptRoot/package.ps1" }
    & "$PSScriptRoot/build-site.ps1"
    if (-not $Publish) { Write-Host 'Prepared locally. No release published.'; return }
    if ((git status --porcelain) -or (git rev-parse HEAD) -ne $revision) { throw 'Source changed during preparation.' }
    $remote = git ls-remote origin refs/heads/master
    if ($LASTEXITCODE -or ($remote -split '\s+')[0] -ne $revision) { throw 'Remote source changed during preparation.' }
    $assets = @(
        "artifacts/publish/VolturaTextClock-Setup-$version-win-x64.exe",
        "artifacts/publish/VolturaTextClock-Setup-$version-win-x64-full.exe",
        "artifacts/publish/VolturaTextClock-$version-win-x64.zip",
        "artifacts/publish/VolturaTextClock-Update-$version.json",
        "artifacts/publish/VolturaTextClock-Update-$version.sig"
    )
    foreach ($asset in $assets) { if (-not (Test-Path -LiteralPath $asset)) { throw "Missing release asset: $asset" } }
    gh release create "v$version" @assets --repo $repository --target $revision --title "Voltura TextClock $version" --notes-file $notes --draft
    if ($LASTEXITCODE) { throw 'Draft release creation failed; nothing was published.' }
    gh release edit "v$version" --repo $repository --draft=false --latest
    if ($LASTEXITCODE) { throw 'Publication failed; inspect the draft release before retrying.' }
    git fetch origin "refs/tags/v${version}:refs/tags/v${version}"
    if ($LASTEXITCODE) { throw 'Published but could not fetch the release tag.' }
    if ((git rev-parse "v$version^{commit}") -ne $revision) { throw 'Published tag does not match source.' }
    Write-Host "Published Voltura TextClock $version from $revision"
} finally { Pop-Location }
