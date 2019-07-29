using Core.Application.Domain;
using Core.Users.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Organizations.Domain
{
    public class OrganizationRoleUser : DomainBase
    {
        public OrganizationRoleUser()
        {
            IsActive = true;
            IsSuspended = false;
        }
        public long OrganizationId { get; set; }
        [ForeignKey("Id")]
        public virtual Organization Organization{ get; set; }

        public long RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role{ get; set; }

        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserLogin UserLogin { get; set; }

        public bool IsActive { get; set; }
        public bool IsSuspended { get; set; }
    }
}
