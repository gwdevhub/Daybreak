using Microsoft.Extensions.Logging;
using System;

namespace Daybreak.Models.Notifications;

public interface INotification
{
    public LogLevel Level { get; }
    public string Title { get; }
    public string Description { get; }
    public Action? OnClick { get; }
}
