Write-Host "Retrieving version"
$filepath = Get-ChildItem -Path .\Daybreak -Filter *.version
$version = $filepath.BaseName
Write-Host "Version: $version"
return $version