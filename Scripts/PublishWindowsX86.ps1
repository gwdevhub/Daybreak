# Publishes the Daybreak.Injector (x86) and Daybreak.API (x86) for Windows.
# These are x86 components that can only be built on Windows.

Param(
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"
$RepoRoot = Resolve-Path "$PSScriptRoot/.."

$InjectorOutput = Join-Path $RepoRoot "Publish/Injector"
$ApiOutput = Join-Path $RepoRoot "Publish/Api"

Write-Host "=== Restoring packages ===" -ForegroundColor Cyan
dotnet restore "$RepoRoot/Daybreak.Injector/Daybreak.Injector.csproj" -r win-x86
if ($LASTEXITCODE -ne 0) { throw "Daybreak.Injector restore failed" }
dotnet restore "$RepoRoot/Daybreak.API/Daybreak.API.csproj" -r win-x86
if ($LASTEXITCODE -ne 0) { throw "Daybreak.API restore failed" }

Write-Host "=== Publishing Daybreak.Injector (win-x86) ===" -ForegroundColor Cyan
dotnet publish "$RepoRoot/Daybreak.Injector/Daybreak.Injector.csproj" -c $Configuration -r win-x86 -o $InjectorOutput --no-restore
if ($LASTEXITCODE -ne 0) { throw "Daybreak.Injector publish failed" }

Write-Host "=== Publishing Daybreak.API (win-x86) ===" -ForegroundColor Cyan
dotnet publish "$RepoRoot/Daybreak.API/Daybreak.API.csproj" -c $Configuration -r win-x86 -o $ApiOutput --no-restore
if ($LASTEXITCODE -ne 0) { throw "Daybreak.API publish failed" }

Write-Host "=== Windows x86 publish complete ===" -ForegroundColor Green
Write-Host "Injector: $InjectorOutput"
Write-Host "API:      $ApiOutput"
