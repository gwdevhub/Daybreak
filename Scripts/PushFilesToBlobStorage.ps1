Param(
    [Parameter(Mandatory=$true)]
    [string]$version,
    [Parameter(Mandatory=$true)]
    [string]$connectionString,
    [Parameter(Mandatory=$true)]
    [string]$sourcePath
)

# Create a new blob container
$containerName = "v$($version.ToLower().Replace('.', '-'))" # Container names must be lowercase
Write-Host "Creating container $($containerName)"
az storage container create --name $containerName --connection-string $connectionString

Write-Host "Uploading files"
# Upload all files from the source path to the blob container
az storage blob upload-batch --destination $containerName --source $sourcePath --connection-string $connectionString

Write-Host "Setting public access to container"
az storage container set-permission --name $containerName --public-access blob --connection-string $connectionString