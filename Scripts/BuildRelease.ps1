Param(
    [Parameter(Mandatory=$true)]
    [string]$version
) #end param

function Get-FileMetadata {
    param (
        [string]$Path
    )

    $fileInfo = Get-Item $Path

    # Temporarily change current location to the folder path
    $currentLocation = Get-Location
    Set-Location -Path .\Publish
    $relativePath = Resolve-Path -Path $Path -Relative
    Set-Location -Path $currentLocation
    $versionInfo = $fileInfo.VersionInfo.ProductVersion
    return @{
        Name         = $fileInfo.Name
        Size         = $fileInfo.Length
        RelativePath = $relativePath.trim(".\\")
        VersionInfo  = $versionInfo
    }
}

$zipPath = "Publish\daybreakv$version.zip"
Write-Output "Deleting pdb file"
Remove-item .\Publish\Daybreak.pdb
Remove-item .\Publish\Daybreak.Installer.pdb
Remove-item .\Publish\Daybreak.7ZipExtractor.pdb
Move-Item -Path .\Publish\Daybreak.Installer.exe -Destination .\Publish\Daybreak.Installer.Temp.exe

Compress-Archive .\Publish\* $zipPath -Force

$files = Get-ChildItem -Path .\Publish -Recurse -File
$metadata = $files | ForEach-Object { Get-FileMetadata $_.FullName }
$json = $metadata | ConvertTo-Json
$json | Out-File -FilePath ".\Publish\Metadata.json"
Write-Host "Metadata written to .\Publish\Metadata.json"