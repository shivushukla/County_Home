using Core.Application;
using Core.Users.VIewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Application.Impl
{
    public class SessionContext : ISessionContext
    {
        public UserSessionModel UserSession { get; set; }
        public string SessionId { get; set; }
    }
}
