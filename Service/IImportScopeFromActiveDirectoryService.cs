using ShareAudit.Model;
using System.Threading.Tasks;

namespace ShareAudit.Service;

public interface IImportScopeFromActiveDirectoryService
{
    Task<string> Import(string domain, string username, string password, ImportComputerType importComputerType);

    Task<string> Import(string domain, ImportComputerType importComputerType);
}
