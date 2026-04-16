namespace ShareAudit.Service;

public interface IPortScanService
{
    bool IsTcpPortOpen(string host, ushort port, int millisecondsTimeout);
}
