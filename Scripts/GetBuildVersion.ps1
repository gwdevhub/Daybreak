Function GetBuildVersion{
    Write-Host "Retrieving version"
    $filepath = Get-ChildItem -Path .\Daybreak -Filter *.version
    Write-Host $filepath
    $version = $filepath.BaseName
    Write-Host "Version: $version"
    return $version | Out-String
}