using Core.Application.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Organizations.Domain
{
    public class Organization : DomainBase
    {
        public Organization()
        {
            IsActive = true;
            IsSuspended = false;
        }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Alias { get; set; }

        public string Email { get; set; }

        public long OrganizationtypeId { get; set; }
        [ForeignKey("OrganizationtypeId")]
        public virtual Lookup OrganizationType { get; set; }

        public string Website { get; set; }

        public string About { get; set; }

        public bool IsActive { get; set; }

        public bool IsSuspended { get; set; }

        public virtual ICollection<OrganizationRoleUser> OrganizationRoleUsers { get; set; }
        public virtual ICollection<OrganizationAddress> OrganizationAddresses { get; set; }

    }
}
