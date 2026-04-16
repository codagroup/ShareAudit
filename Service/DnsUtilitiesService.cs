using System.Net;
using System.Net.Sockets;

namespace ShareAudit.Service;

public class DnsUtilitiesService : IDnsUtilitiesService
{
    public string GetPtrRecord(IPAddress ipAddress)
    {
        try
        {
            var ptrRecord = Dns.GetHostEntry(ipAddress).HostName;
            if (TryResolveHost(ptrRecord, out var resolvedIPAddresses))
            {
                if (resolvedIPAddresses.Contains(ipAddress))
                {
                    return ptrRecord;
                }
            }
        }
        catch (SocketException)
        {
        }

        return string.Empty;
    }

    public bool TryResolveHost(string host, out IEnumerable<IPAddress> ipAddresses)
    {
        host = host ?? throw new ArgumentNullException(nameof(host));

        try
        {
            ipAddresses = Dns.GetHostAddresses(host);
            return true;
        }
        catch (SocketException ex) when (ex.Message == "No such host is known")
        {
            ipAddresses = [];
            return false;
        }
    }
}
