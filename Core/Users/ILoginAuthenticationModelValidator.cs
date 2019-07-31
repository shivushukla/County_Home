using Core.Organizations.Domain;

namespace Core.Users
{
    public interface ILoginAuthenticationModelValidator
    {
        (bool IsSucceeded, OrganizationRoleUser orgRoleUser, string Error) IsValid(LoginAuthenticationModel model, bool isDeviceRequest = false);
    }
}
