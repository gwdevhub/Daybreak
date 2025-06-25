using Microsoft.Extensions.Options;
using System;

namespace Daybreak.Shared.Models;
public sealed class StaticOptionsMonitor<T>(T currentValue)
    : IOptionsMonitor<T>
{
    public T CurrentValue { get; } = currentValue;

    public T Get(string? name) => this.CurrentValue;

    public IDisposable? OnChange(Action<T, string?> listener) => EmptyDisposable.Instance;

    private sealed class EmptyDisposable : IDisposable
    {
        public static readonly EmptyDisposable Instance = new();
        public void Dispose() { }
    }
}
