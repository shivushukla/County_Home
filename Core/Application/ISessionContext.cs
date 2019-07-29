using Core.Users.VIewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application
{
    public interface ISessionContext
    {
        UserSessionModel UserSession { get; set; }
        string SessionId { get; set; }
    }
}
