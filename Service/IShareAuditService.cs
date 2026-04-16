using ShareAudit.Model;

namespace ShareAudit.Service;

public interface IShareAuditService
{
    event EventHandler Started;

    event EventHandler Stopped;

    void StartAudit(Project project);

    void StopAudit();
}
