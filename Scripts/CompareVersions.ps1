Param(
    [Parameter(Mandatory=$true)]
    [string]$currentVersion,
    [Parameter(Mandatory=$true)]
    [string]$lastVersion
)

if ($currentVersion.StartsWith("v")){
    $currentVersion = $currentVersion.Substring(1)
}

if ($lastVersion.StartsWith("v")){
    $lastVersion = $lastVersion.Substring(1)
}

$isNewer = $currentVersion.CompareTo($lastVersion) -eq 1
if ($isNewer -eq $false){
    throw "Version is not incremented. Current version " + $currentVersion + ". Last version " + $lastVersion
}
else{
    Write-Host "Version has been incremented"
}