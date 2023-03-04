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

$currentVersionTokens = $currentVersion.Split('.')
$lastVersionTokens = $lastVersion.Split('.')
if ($currentVersionTokens.Length -eq $lastVersionTokens.Length)
{
    for($i = 0 ; $i -lt $currentVersionTokens.Length; $i++)
    {
        $currentVersionToken = [int]$currentVersionTokens[$i]
        $lastVersionToken = [int]$lastVersionTokens[$i]
        if ($currentVersionToken -lt $lastVersionToken){
            throw "Version is not incremented. Current version " + $currentVersion + ". Last version " + $lastVersion
        }
    }

    if ($currentVersionToken -le $lastVersionToken){
        throw "Version is not incremented. Current version " + $currentVersion + ". Last version " + $lastVersion
    }

    Write-Host "Version has been incremented"
}
elseif ($currentVersionTokens.Length -gt $lastVersionTokens.Length){
    if ($currentVersionTokens[$lastVersionTokens.Length - 1] -lt $lastVersionTokens[$lastVersionTokens.Length - 1]){
        throw "Version is not incremented. Current version " + $currentVersion + ". Last version " + $lastVersion
    }

    Write-Host "Version has been incremented"
}
elseif ($currentVersionTokens.Length -lt $lastVersionTokens.Length){
    if ($currentVersionTokens[$currentVersionTokens.Length - 1] -le $lastVersionTokens[$currentVersionTokens.Length - 1]){
        throw "Version is not incremented. Current version " + $currentVersion + ". Last version " + $lastVersion
    }

    Write-Host "Version has been incremented"
}