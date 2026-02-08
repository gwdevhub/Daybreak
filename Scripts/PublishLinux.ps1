# Publishes the main Daybreak Linux x64 build and the Linux x64 installer.

Param(
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"
$RepoRoot = Resolve-Path "$PSScriptRoot/.."

$DaybreakOutput = Join-Path $RepoRoot "Publish"
$InstallerOutput = Join-Path $RepoRoot "Publish/Installer"

Write-Host "=== Publishing Daybreak.Linux (linux-x64) ===" -ForegroundColor Cyan
dotnet publish "$RepoRoot/Daybreak.Linux/Daybreak.Linux.csproj" -c $Configuration -r linux-x64 -o $DaybreakOutput
if ($LASTEXITCODE -ne 0) { throw "Daybreak.Linux publish failed" }

Write-Host "=== Publishing Daybreak.Installer (linux-x64) ===" -ForegroundColor Cyan
dotnet publish "$RepoRoot/Daybreak.Installer/Daybreak.Installer.csproj" -c $Configuration -r linux-x64 -o $InstallerOutput
if ($LASTEXITCODE -ne 0) { throw "Daybreak.Installer publish failed" }

Write-Host "=== Linux x64 publish complete ===" -ForegroundColor Green
Write-Host "Daybreak:  $DaybreakOutput"
Write-Host "Installer: $InstallerOutput"
