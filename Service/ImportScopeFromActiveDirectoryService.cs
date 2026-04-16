using ShareAudit.Model;
using System.DirectoryServices;
using AD = System.DirectoryServices;

namespace ShareAudit.Service;

public class ImportScopeFromActiveDirectoryService : IImportScopeFromActiveDirectoryService
{
    public async Task<string> Import(string domain, string username, string password, ImportComputerType importComputerType)
    {
        return await Task.Run(() =>
        {
            var computerNames = new List<string>();

            using (AD.DirectoryEntry directoryEntry = new($"LDAP://{domain}", username, password))
            {
                using AD.DirectorySearcher directorySearcher = new(directoryEntry);
                switch (importComputerType)
                {
                    case ImportComputerType.Servers:
                        directorySearcher.Filter = "(&(objectClass=computer)(operatingSystem=Windows Server*))";
                        break;

                    case ImportComputerType.Workstations:
                        directorySearcher.Filter = "(&(objectClass=computer)(|(operatingSystem=Windows XP*)(operatingSystem=Windows Vista*)(operatingSystem=Windows 7*)(operatingSystem=Windows 8*)(operatingSystem=Windows 10*)))";
                        break;

                    case ImportComputerType.All:
                        directorySearcher.Filter = "(objectClass=computer)";
                        break;
                }

                directorySearcher.SizeLimit = 0;
                directorySearcher.PageSize = 250;
                directorySearcher.PropertiesToLoad.Add("dNSHostName");

                using AD.SearchResultCollection searchResultCollection = directorySearcher.FindAll();
                foreach (AD.SearchResult searchResult in searchResultCollection)
                {
                    if (searchResult.Properties["dNSHostName"].Count > 0)
                    {
                        computerNames.Add((searchResult.Properties["dNSHostName"][0] as string)!);
                    }
                }
            }
            return string.Join(", ", computerNames);
        });
    }

    public async Task<string> Import(string domain, ImportComputerType importComputerType)
    {
        return await Task.Run(() =>
        {
            List<string> computerNames = [];

            using (AD.DirectoryEntry directoryEntry = new($"LDAP://{domain}"))
            {
                using AD.DirectorySearcher directorySearcher = new(directoryEntry);
                switch (importComputerType)
                {
                    case ImportComputerType.Servers:
                        directorySearcher.Filter = "(&(objectClass=computer)(operatingSystem=Windows Server*))";
                        break;

                    case ImportComputerType.Workstations:
                        directorySearcher.Filter = "(&(objectClass=computer)(|(operatingSystem=Windows XP*)(operatingSystem=Windows Vista*)(operatingSystem=Windows 7*)(operatingSystem=Windows 8*)(operatingSystem=Windows 10*)))";
                        break;

                    case ImportComputerType.All:
                        directorySearcher.Filter = "(objectClass=computer)";
                        break;
                }

                directorySearcher.SizeLimit = 0;
                directorySearcher.PageSize = 250;
                directorySearcher.PropertiesToLoad.Add("dNSHostName");

                using SearchResultCollection searchResultCollection = directorySearcher.FindAll();
                foreach (AD.SearchResult searchResult in searchResultCollection)
                {
                    if (searchResult.Properties["dNSHostName"].Count > 0)
                    {
                        computerNames.Add((searchResult.Properties["dNSHostName"][0] as string)!);
                    }
                }
            }
            return string.Join(", ", computerNames);
        });
    }
}
