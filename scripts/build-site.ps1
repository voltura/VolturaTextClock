#Requires -Version 7.0
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
[xml]$metadata = Get-Content (Join-Path $root 'Directory.Build.props')
$version = [string]$metadata.Project.PropertyGroup.Version
$output = Join-Path $root 'artifacts/site'
New-Item -ItemType Directory -Path $output -Force | Out-Null
Copy-Item -Path "$root/site/*" -Destination $output -Recurse -Force
$html = [IO.File]::ReadAllText((Join-Path $output 'index.html')).Replace('{{VERSION}}', $version)
if ($html.Contains('{{')) { throw 'Unresolved website template marker.' }
[IO.File]::WriteAllText((Join-Path $output 'index.html'), $html)
[IO.File]::WriteAllText((Join-Path $output 'release.json'), (ConvertTo-Json @{ version = $version; release = "https://github.com/voltura/VolturaTextClock/releases/tag/v$version" }))
Write-Host "Built website for $version in $output"
