using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Daybreak.Services.Telemetry;

internal sealed class SwappableLoggerProvider()
    : ILoggerProvider, ISupportExternalScope
{
    private readonly ConcurrentDictionary<string, SwappableLogger> loggers = new();

    private IExternalScopeProvider? scopeProvider;
    private volatile ILoggerProvider? inner;   // null = telemetry OFF

    public void SetInner(ILoggerProvider? provider)
    {
        var previous = Interlocked.Exchange(ref this.inner, provider);
        if (provider is ISupportExternalScope s && this.scopeProvider is not null)
        {
            s.SetScopeProvider(this.scopeProvider);
        }

        foreach (var l in this.loggers.Values)
        {
            l.SetInner(provider?.CreateLogger(l.Category));
        }

        previous?.Dispose();
    }

    public ILogger CreateLogger(string categoryName)
    {
        return this.loggers.GetOrAdd(categoryName, name =>
        {
            var innerLogger = this.inner?.CreateLogger(name);
            return new SwappableLogger(name, innerLogger);
        });
    }

    public void Dispose() => this.SetInner(null);

    public void SetScopeProvider(IExternalScopeProvider provider)
    {
        this.scopeProvider = provider;
        if (this.inner is ISupportExternalScope s)
        {
            s.SetScopeProvider(provider);
        }
    }
}

internal sealed class SwappableLogger
    : ILogger
{
    private volatile ILogger? inner;

    public string Category { get; }

    internal SwappableLogger(string category, ILogger? inner)
    {
        this.Category = category;
        this.inner = inner;
    }

    internal void SetInner(ILogger? logger) => this.inner = logger;

    IDisposable ILogger.BeginScope<TState>(TState state) => this.inner?.BeginScope(state) ?? NullScope.Instance;

    public bool IsEnabled(LogLevel level) => this.inner?.IsEnabled(level) ?? false;

    public void Log<TState>(LogLevel level, EventId id,
        TState state, Exception? ex, Func<TState, Exception?, string> formatter)
    {
        this.inner?.Log(level, id, state, ex, formatter);
    }

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();
        public void Dispose() { }
    }
}
