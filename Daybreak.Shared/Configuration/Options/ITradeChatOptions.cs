namespace Daybreak.Configuration.Options;

public interface ITradeChatOptions
{
    string HttpsUri { get; set; }

    string WssUri { get; set; }
}
