namespace Daybreak.Models.Interop;

public readonly struct InstanceContext
{
    public const int BaseOffset = 0x01AC;
    
    /// <summary>
    /// Milliseconds since the instance was joined.
    /// </summary>
    public readonly uint Timer;
}
