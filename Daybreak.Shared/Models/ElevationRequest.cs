using System;

namespace Daybreak.Shared.Models;

public sealed class ElevationRequest
{
    public object? DataContext { get; set; }
    public Type? View { get; set; }
    public string? MessageToUser { get; set; }
}
