using Core.Application;
using Core.Application.Attributes;
using Core.Application.ViewModel;
using Core.Organizations;
using Core.Organizations.Domain;
using Core.Users.Domain;
using Core.Users.VIewModel;
using FluentValidation;

namespace Core.Users.Impl
{
    [DefaultImplementation(Interface = typeof(IValidator<LoginAuthenticationModel>))]
    public class LoginAuthenticationModelValidator : AbstractValidator<LoginAuthenticationModel>, ILoginAuthenticationModelValidator
    {
        private readonly IRepository<Person> _personRepository;
        private readonly IUserLoginService _userLoginService;
        readonly IOrganizationRoleUserRepository _organizationRoleUserRepository;
        public LoginAuthenticationModelValidator(IUserLoginService userLoginService, IUnitOfWork unitOfWork)
        {
            _userLoginService = userLoginService;
            _personRepository = unitOfWork.Repository<Person>();
            _organizationRoleUserRepository = (IOrganizationRoleUserRepository)unitOfWork.Repository<OrganizationRoleUser>();
        }

        public (bool IsSucceeded, OrganizationRoleUser orgRoleUser, string Error) IsValid(LoginAuthenticationModel model, bool isDeviceRequest = false)
        {
            var userLogin = _userLoginService.GetbyUserName(model.Username);
            if (userLogin == null)
            {
                model.Message = FeedbackMessageModel.CreateErrorMessage("Requested User Not Found."); //Messages.InvalidEmailNotInRecords
                return (false, null, "Invalid login credentials.");//Messages.InvalidCredentials
            }
            if (userLogin.IsLocked)
            {
                model.Message = FeedbackMessageModel.CreateErrorMessage("Your account has been locked due to many incorrect login attempts. Please contact to your administrator to get Your account has been locked due to many incorrect login attempts. Please contact to your administrator to get Your account has been locked due to many incorrect login attempts. Please contact to your administrator to get your account unlocked."); //Messages.AccountLocked
                return (false, null, "");
            }

            var person = _personRepository.Get(userLogin.Id);
            if (!person.IsActive)
            {
                model.Message = FeedbackMessageModel.CreateErrorMessage("Your account has been deactivated, please contact to your administrator.");//Messages.UserDeactivated
                return (false, null, "Your account has been deactivated, please contact to your administrator.");//Messages.UserDeactivated
            }
            var orgRoleUser = _organizationRoleUserRepository.GetForUser(userLogin.Id);
            userLogin = _userLoginService.UpdateforInvalidAttempt(userLogin);

            if (userLogin.IsLocked)
            {
                model.Message = FeedbackMessageModel.CreateErrorMessage("Your account has been locked due to many incorrect login attempts. Please contact to your administrator to get Your account has been locked due to many incorrect login attempts. Please contact to your administrator to get Your account has been locked due to many incorrect login attempts. Please contact to your administrator to get your account unlocked."); //Messages.AccountLocked
            }
            else if (userLogin.LoginAttemptCount == (UserLogin.MaxAttempts - 2))
            {
                model.Message = FeedbackMessageModel.CreateErrorMessage("Invalid username/password. You have got 2 more attempts left before your account gets locked. Please use carefully."); //Messages.InvalidCredentialsLastTwoAttempts
            }
            else if (userLogin.LoginAttemptCount == (UserLogin.MaxAttempts - 1))
            {
                model.Message = FeedbackMessageModel.CreateErrorMessage("Invalid login credentials. Your account will be locked after 1 more incorrect attempt."); //Messages.LastLoginAttempt
            }
            else
            {
                model.Message = FeedbackMessageModel.CreateErrorMessage("Invaid password"); //Messages.InvalidPassword
            }

            return (false, null, model.Message.Message); ;
        }
    }
}
