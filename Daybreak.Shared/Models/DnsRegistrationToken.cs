using MeaMod.DNS.Multicast;
using System;

namespace Daybreak.Shared.Models;
public readonly struct DnsRegistrationToken : IDisposable
{
    private readonly ServiceDiscovery serviceDiscovery = new();

    internal DnsRegistrationToken(
        ServiceProfile serviceProfile)
    {
        this.serviceDiscovery.Advertise(serviceProfile);
        this.serviceDiscovery.Announce(serviceProfile);
    }

    public void Dispose()
    {
        this.serviceDiscovery.Dispose();
    }
}
