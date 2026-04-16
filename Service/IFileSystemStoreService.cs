using ShareAudit.Model;

namespace ShareAudit.Service;

public interface IFileSystemStoreService
{
    string ExportDefaultFilename { get; }

    string ExportFilter { get; }

    string ShareAuditDefaultFilename { get; }

    string ShareAuditFilter { get; }

    Task CreateProjectAsync(string path);

    Task ExportProjectAsync(Project project, string path);

    Task<Project> LoadProjectAsync(string path);

    Task SaveProjectAsync(Project project, string path);
}
