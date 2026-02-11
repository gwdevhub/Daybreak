using Daybreak.Shared.Models.Async;
using Daybreak.Shared.Models.Mods;
using Daybreak.Shared.Services.Mods;
using Daybreak.Shared.Services.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimplePlugin.Options;
using System.Core.Extensions;
using System.Extensions.Core;

namespace SimplePlugin.Services;

public sealed class SimpleNotificationMod(
    INotificationService notificationService,
    IOptionsMonitor<SimpleOptions> simpleOptions,
    ILogger<SimpleNotificationMod> logger)
    : IModService
{
    private readonly INotificationService notificationService = notificationService.ThrowIfNull();
    private readonly IOptionsMonitor<SimpleOptions> simpleOptions = simpleOptions.ThrowIfNull();
    private readonly ILogger<SimpleNotificationMod> logger = logger.ThrowIfNull();

    public string Name { get; } = "Simple Notification Mod";
    public string Description { get; } = "A simple mod that shows notifications on Guild Wars application lifetime.";
    public bool IsEnabled { get; set; } = true;
    public bool IsInstalled { get; } = true;
    public bool IsVisible { get; } = true;
    public bool CanCustomManage { get; } = false;
    public bool CanUninstall { get; } = false;
    public bool CanDisable { get; } = true;

    public IEnumerable<string> GetCustomArguments() => [];

    public Task<bool> IsUpdateAvailable(CancellationToken cancellationToken) => Task.FromResult(false);

    public Task OnCustomManagement(CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsCreated(GuildWarsCreatedContext guildWarsCreatedContext, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var notificationToken = this.notificationService.NotifyInformation(
            title: nameof(this.OnGuildWarsCreated),
            description: $"You've been notified by {nameof(SimpleNotificationMod)}!");

        
        scopedLogger.LogWarning("Find me in the logs!");
        return Task.CompletedTask;
    }

    public Task OnGuildWarsRunning(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnGuildWarsStarted(GuildWarsStartedContext guildWarsStartedContext, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var notificationToken = this.notificationService.NotifyInformation(
            title: nameof(this.OnGuildWarsStarted),
            description: $"{nameof(SimpleNotificationMod)} is under your bed!");


        scopedLogger.LogWarning("No seriously, where am I?");
        return Task.CompletedTask;
    }

    public Task OnGuildWarsStarting(GuildWarsStartingContext guildWarsStartingContext, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var notificationToken = this.notificationService.NotifyInformation(
            title: nameof(this.OnGuildWarsStarting),
            description: $"{nameof(SimpleNotificationMod)} is looking straight at you!");


        scopedLogger.LogWarning("Peekaboo!");
        return Task.CompletedTask;
    }

    public Task OnGuildWarsStartingDisabled(GuildWarsStartingDisabledContext guildWarsStartingDisabledContext, CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        var notificationToken = this.notificationService.NotifyInformation(
            title: nameof(this.OnGuildWarsStartingDisabled),
            description: $"{nameof(SimpleNotificationMod)}: How did you manage that?");


        scopedLogger.LogWarning("You're no fun!");
        return Task.CompletedTask;
    }

    public IProgressAsyncOperation<bool> PerformInstallation(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public IProgressAsyncOperation<bool> PerformUninstallation(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> PerformUpdate(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ShouldRunAgain(GuildWarsRunningContext guildWarsRunningContext, CancellationToken cancellationToken) => Task.FromResult(false);
}
