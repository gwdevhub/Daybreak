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

# Compare versions segment by segment
$maxLength = [Math]::Max($currentVersionTokens.Length, $lastVersionTokens.Length)
for($i = 0; $i -lt $maxLength; $i++)
{
    # Treat missing segments as 0
    $currentVersionToken = if ($i -lt $currentVersionTokens.Length) { [int]$currentVersionTokens[$i] } else { 0 }
    $lastVersionToken = if ($i -lt $lastVersionTokens.Length) { [int]$lastVersionTokens[$i] } else { 0 }
    
    if ($currentVersionToken -gt $lastVersionToken) {
        # Current version is greater, we're done
        Write-Host "Version has been incremented"
        exit 0
    }
    elseif ($currentVersionToken -lt $lastVersionToken) {
        # Current version is less, fail
        throw "Version is not incremented. Current version " + $currentVersion + ". Last version " + $lastVersion
    }
    # If equal, continue to next segment
}

# All segments are equal - version is not incremented
throw "Version is not incremented. Current version " + $currentVersion + ". Last version " + $lastVersion