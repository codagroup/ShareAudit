namespace ShareAudit.Service;

public interface IScopeExpansionService
{
    IEnumerable<(string ipAddress, string fqdn)> ExpandScope(string scopeText, bool doNotExpandNamesToIPs);
}
