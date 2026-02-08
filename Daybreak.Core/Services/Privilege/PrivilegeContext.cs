namespace Daybreak.Services.Privilege;
public sealed class PrivilegeContext
{
    public Type? CancelViewType { get; set; }
    public string? UserMessage { get; set; }
    public TaskCompletionSource<bool>? PrivilegeRequestOperation { get; set; }
    public (string, object)[]? CancelViewParams { get; set; }
}
