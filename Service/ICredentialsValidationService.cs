using ShareAudit.Model;

namespace ShareAudit.Service;

public interface ICredentialsValidationService
{
    Task<(string domain, string username)> GetCurrentUserInformation();

    Task<(bool isValid, string errorMessage)> ValidateCredentialsAsync(Credentials credentials);

    Task<(bool isValid, string errorMessage)> ValidateDomainAsync(string domain);

    Task<(bool isValid, string errorMessage)> ValidatePasswordAsync(string password);

    Task<(bool isValid, string errorMessage)> ValidateUsernameAsync(string username);
}
