using Microsoft.Extensions.Logging;
using Realms;
using System;

namespace Daybreak.Services.Logging.Models;
internal sealed class LogDTO : RealmObject
{
    [PrimaryKey]
    public string? Id { get; set; } = Guid.NewGuid().ToString();
    public string? Message { get; set; }
    public string? Category { get; set; }
    public int LogLevel { get; set; }
    public string? CorrelationVector { get; set; }
    public string? EventId { get; set; }
    public DateTimeOffset LogTime { get; set; }
}
