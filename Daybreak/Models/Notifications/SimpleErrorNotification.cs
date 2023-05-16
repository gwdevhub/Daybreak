using Microsoft.Extensions.Logging;
using System;

namespace Daybreak.Models.Notifications;
public sealed class SimpleErrorNotification : INotification
{
    public LogLevel Level => LogLevel.Error;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Action? OnClick { get; init; } = default!;
}
