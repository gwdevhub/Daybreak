using Daybreak.API.Interop.GuildWars;

namespace Daybreak.API.Models;

public struct DecodeStringContext(WrappedPointer<char> encodedString, TaskCompletionSource<string?> taskCompletionSource)
{
    public WrappedPointer<char> EncodedString = encodedString;
    public TaskCompletionSource<string?> TaskCompletionSource = taskCompletionSource;
}
