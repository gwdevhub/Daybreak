namespace Daybreak.Models
{
    public sealed class LoginCredentials
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? CharacterName { get; set; }
        public bool Default { get; set; }
    }
}
