namespace ShareAudit.Service;

public interface IScopeValidationService
{
    Task<(bool isValid, string errorMessage)> ValidateScopeAsync(string scope);
}
