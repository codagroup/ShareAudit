namespace ShareAudit.Service;

public interface ISidUtilitiesService
{
    string SidStringToAccountName(string host, string sid);
}
