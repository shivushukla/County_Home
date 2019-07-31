using Core.Users.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Users
{
    public interface IUserLoginService
    {
        UserLogin UpdateforInvalidAttempt(UserLogin login);
        UserLogin GetbyUserName(string userName);
        UserLogin GetbyUserId(long userId);
        UserLogin UpdateforValidAttempt(UserLogin login);
        bool IsUniqueUserName(string userName, long userId = 0);
        bool IsUniqueEmailAddress(string email, long userId = 0);
    }
}
