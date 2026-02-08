using Daybreak.Shared.Models;

namespace Daybreak.Models;
public sealed class CredentialModel
{
    public required LoginCredentials LoginCredentials { get; init; }
    public bool PasswordVisible { get; set; } = false;
}
