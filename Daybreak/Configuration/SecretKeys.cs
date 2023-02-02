namespace Daybreak.Configuration;

public class SecretKeys
{
    public static readonly SecretKeys AadApplicationId = new() { Key = "AadApplicationId" };
    public static readonly SecretKeys AadTenantId = new() { Key = "AadTenantId" };

    public string Key { get; private set; } = string.Empty;
    private SecretKeys()
    {
    }
}
