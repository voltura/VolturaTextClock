#Requires -Version 7.0
$ErrorActionPreference = 'Stop'
Push-Location (Split-Path $PSScriptRoot -Parent)
try {
    dotnet build VolturaTextClock.sln -c Release --nologo -warnaserror
    if ($LASTEXITCODE) { throw 'Build failed.' }
    dotnet run --project tests/ClockSmoke/ClockSmoke.csproj -c Release
    if ($LASTEXITCODE) { throw 'Clock smoke checks failed.' }
    dotnet run --project tests/Reliability/Reliability.csproj -c Release
    if ($LASTEXITCODE) { throw 'Reliability checks failed.' }
    dotnet list VolturaTextClock/VolturaTextClock.csproj package --vulnerable --include-transitive
    if ($LASTEXITCODE) { throw 'Dependency audit failed.' }
} finally { Pop-Location }
