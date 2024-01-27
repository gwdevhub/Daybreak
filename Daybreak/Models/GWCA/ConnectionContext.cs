namespace Daybreak.Models.GWCA;

public readonly struct ConnectionContext
{
    public readonly int Port;
    public readonly uint ProcessId;

    public ConnectionContext(int port, uint processId)
    {
        this.Port = port;
        this.ProcessId = processId;
    }
}
