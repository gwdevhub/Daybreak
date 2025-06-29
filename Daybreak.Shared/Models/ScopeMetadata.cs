using Microsoft.CorrelationVector;

namespace Daybreak.Shared.Models;

public class ScopeMetadata(CorrelationVector correlationVector)
{
    public CorrelationVector CorrelationVector { get; set; } = correlationVector;
}