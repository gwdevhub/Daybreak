# Publishes the main Daybreak Windows x64 build and the Windows x64 installer.

Param(
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"
$RepoRoot = Resolve-Path "$PSScriptRoot/.."

$DaybreakOutput = Join-Path $RepoRoot "Publish"
$InstallerOutput = Join-Path $RepoRoot "Publish/Installer"

Write-Host "=== Publishing Daybreak.Windows (win-x64) ===" -ForegroundColor Cyan
dotnet publish "$RepoRoot/Daybreak.Windows/Daybreak.Windows.csproj" -c $Configuration -r win-x64 -o $DaybreakOutput
if ($LASTEXITCODE -ne 0) { throw "Daybreak.Windows publish failed" }

Write-Host "=== Publishing Daybreak.Installer (win-x64) ===" -ForegroundColor Cyan
dotnet publish "$RepoRoot/Daybreak.Installer/Daybreak.Installer.csproj" -c $Configuration -r win-x64 -o $InstallerOutput
if ($LASTEXITCODE -ne 0) { throw "Daybreak.Installer publish failed" }

Write-Host "=== Windows x64 publish complete ===" -ForegroundColor Green
Write-Host "Daybreak:  $DaybreakOutput"
Write-Host "Installer: $InstallerOutput"
