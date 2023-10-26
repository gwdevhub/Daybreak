namespace Daybreak.Services.Scanner.Models;

internal class AttributePayload
{
    public uint Id { get; set; }
    public uint ActualLevel { get; set; }
    public uint BaseLevel { get; set; }
    public uint IncrementPoints { get; set; }
    public uint DecrementPoints { get; set; }
}
