using System.Net;
using System.Net.Sockets;


namespace ShareAudit.Service;

public class ScopeExpansionService : IScopeExpansionService
{
    private readonly IDnsUtilitiesService _dnsUtilitiesService;

    public ScopeExpansionService(
        IDnsUtilitiesService dnsUtilitiesService)
    {
        _dnsUtilitiesService = dnsUtilitiesService ?? throw new ArgumentNullException(nameof(dnsUtilitiesService));
    }

    public IEnumerable<(string ipAddress, string fqdn)> ExpandScope(string scopeText, bool doNotExpandNamesToIPs)
    {
        scopeText = scopeText ?? throw new ArgumentNullException(nameof(scopeText));
        scopeText = scopeText.Replace(" ", string.Empty);
        scopeText = scopeText.Replace("\t", string.Empty);

        var lines = scopeText.Split(",;\r\n".ToCharArray());

        var scope = new HashSet<(string ipAddress, string fqdn)>();

        foreach (var line in lines)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                if (line.Contains("/") && IPNetwork2.TryParse(line, out var ipNetwork))
                {
                    foreach (IPAddress ip in ipNetwork.ListIPAddress())
                    {
                        if (ip.AddressFamily == AddressFamily.InterNetwork && (ip.Equals(ipNetwork.Network) || ip.Equals(ipNetwork.Broadcast)))
                        {
                            continue;
                        }
                        else
                        {
                            scope.Add((ip.ToString(), ip.AddressFamily == AddressFamily.InterNetworkV6 ? $"{ip.ToString().Replace(':', '-')}.ipv6-literal.net" : string.Empty));
                        }
                    }
                }
                else if (IPAddress.TryParse(line, out var ip))
                {
                    scope.Add((ip.ToString(), ip.AddressFamily == AddressFamily.InterNetworkV6 ? $"{ip.ToString().Replace(':', '-')}.ipv6-literal.net" : string.Empty));
                }
                else
                {
                    if (doNotExpandNamesToIPs)
                    {
                        scope.Add((string.Empty, line));
                    }
                    else if (_dnsUtilitiesService.TryResolveHost(line, out var ipAddresses))
                    {
                        foreach (var address in ipAddresses)
                        {
                            scope.Add((address.ToString(), address.AddressFamily == AddressFamily.InterNetworkV6 ? $"{address.ToString().Replace(':', '-')}.ipv6-literal.net" : string.Empty));
                        }
                    }
                }
            }
        }

        return scope;
    }
}