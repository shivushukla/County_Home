using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Core.Users.VIewModel
{
    public class UserSessionModel
    {
        public long UserId { get; set; }

        public long OrganizationRoleUserId { get; set; }
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public long OrganizationType { get; set; }

        public DateTime? LastLoginAt { get; set; }

        [XmlIgnore]
        public long RoleId { get; set; }

        public string RoleAlias { get; set; }
        public string RoleName { get; set; }
    }
}
