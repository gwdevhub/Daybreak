Function GetBuildVersion{
    $filepath = Get-ChildItem -Path .\Daybreak -Filter *.version
    $version = $filepath.BaseName
    return $version | Out-String
}