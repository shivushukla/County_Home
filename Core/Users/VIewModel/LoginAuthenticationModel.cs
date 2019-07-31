

using Core.Application.ViewModel;

namespace Core.Users.VIewModel
{
    public class LoginAuthenticationModel
    {
        public FeedbackMessageModel Message { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
