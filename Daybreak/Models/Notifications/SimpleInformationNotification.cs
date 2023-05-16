using Microsoft.Extensions.Logging;
using System;

namespace Daybreak.Models.Notifications;

public sealed class SimpleInformationNotification : INotification
{
    public LogLevel Level => LogLevel.Information;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Action? OnClick { get; init; } = default!;
}
