Function GetBuildVersion{
    Write-Host "Retrieving version"
    $filepath = Get-ChildItem -Path .\Daybreak -Filter *.version
    $version = $filepath.BaseName
    Write-Host "Version: $version"
    Write-Host "Setting Version variable"
    "Version=$version" | Out-File $GITHUB_ENV -Append
    return $version
}