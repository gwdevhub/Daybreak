namespace Daybreak.Shared.Models.Themes;

public sealed class EmbedBackground(string embedCode)
    : IAppBackground
{
    public string EmbedCode { get; } = embedCode;
}
