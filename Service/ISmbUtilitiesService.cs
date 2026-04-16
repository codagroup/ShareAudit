namespace ShareAudit.Service;

public interface ISmbUtilitiesService
{
    NetUseConnection CreateNetUseConnection(string host, string username, string domain, string password);

    IEnumerable<ShareInfo1> NetShareEnum(string host);
}
