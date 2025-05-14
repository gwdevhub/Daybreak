namespace Daybreak.Shared.Configuration.Options;

public interface ITradeChatOptions
{
    string HttpsUri { get; set; }

    string WssUri { get; set; }
}
