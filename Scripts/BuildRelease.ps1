Param(
    [Parameter(Mandatory=$true)]
    [string]$version
) #end param

Write-Output "Deleting pdb file"
Remove-item .\Publish\Daybreak.pdb
Remove-item .\Publish\Daybreak.Installer.pdb
$zipPath = "Publish\daybreakv$version.zip"
Write-Output "Compressing binaries to $zipPath"
Compress-Archive .\Publish\* $zipPath -Force
Write-Output "Cleaning up files"
foreach ($file in Get-ChildItem -Path .\Publish -Exclude *.zip -Recurse -ErrorAction SilentlyContinue)
{
    Write-Output "Removing $file"
    Remove-item $file -Recurse -Force -ErrorAction SilentlyContinue
}