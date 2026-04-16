using System.Net;

namespace ShareAudit.Service;

public interface IDnsUtilitiesService
{
    string GetPtrRecord(IPAddress ipAddress);

    bool TryResolveHost(string host, out IEnumerable<IPAddress> ipAddresses);
}
