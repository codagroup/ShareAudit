using ShareAudit.Model;
using System.Collections.Generic;

namespace ShareAudit.Service;

public interface IFileSystemOperationService
{
    IEnumerable<AccessRuleEntry> EnumerateAccessRules(string path);

    IEnumerable<DirectoryEntry> EnumerateDirectories(string path);

    IEnumerable<FileEntry> EnumerateFiles(string path);

    EffectiveAccess GetEffectiveAccess(string path);

    string HeadFile(string path, int bytes);
}
