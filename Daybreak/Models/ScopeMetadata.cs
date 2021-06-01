using Microsoft.CorrelationVector;

namespace Daybreak.Models
{
    public class ScopeMetadata
    {
        public CorrelationVector CorrelationVector { get; set; }

        public ScopeMetadata(CorrelationVector correlationVector)
        {
            this.CorrelationVector = correlationVector;
        }
    }
}