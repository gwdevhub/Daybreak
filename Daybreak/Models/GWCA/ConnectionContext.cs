namespace Daybreak.Models.GWCA;

public readonly struct ConnectionContext
{
    public readonly int Port;

    public ConnectionContext(int port)
    {
        this.Port = port;
    }
}
