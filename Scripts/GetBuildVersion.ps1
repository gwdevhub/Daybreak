Function GetBuildVersion{
    Write-Output "Retrieving version"
    $filepath = Get-ChildItem -Path .\Daybreak -Filter *.version
    $version = $filepath.BaseName
    Write-Output "Version: $version"
    return $version
}