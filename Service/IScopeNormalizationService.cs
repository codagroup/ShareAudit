using System.Threading.Tasks;

namespace ShareAudit.Service;

public interface IScopeNormalizationService
{
    Task<string> NormalizeScopeAsync(string scope);
}
