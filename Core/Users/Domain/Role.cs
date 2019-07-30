using Core.Application.Domain;
using Core.Organizations.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Users.Domain
{
    public class Role : DomainBase
    {
        public Role()
        {
            IsActive = true;
            IsSuspended = false;
        }
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Alias { get; set; }

        public string Description { get; set; }

        public long? OrganizationTypeId { get; set; }
        [ForeignKey("OrganizationTypeId")]
        public virtual Lookup OrganizationType { get; set; }

        public bool IsActive { get; set; }
        public bool IsSuspended { get; set; }
    }
}
