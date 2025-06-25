namespace Daybreak.Configuration;

public class SecretKeys
{
    public static readonly SecretKeys AadApplicationId = new() { Key = "AadApplicationId" };
    public static readonly SecretKeys AadTenantId = new() { Key = "AadTenantId" };
    public static readonly SecretKeys ApmApiKey = new() { Key = "ApmApiKey" };
    public static readonly SecretKeys ApmUri = new() { Key = "ApmUri" };

    public string Key { get; private set; } = string.Empty;
    private SecretKeys()
    {
    }
}
