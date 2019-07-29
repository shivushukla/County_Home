using Core.Application.Domain;
using Core.Geo.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Organizations.Domain
{
    public class OrganizationAddress : DomainBase
    {
        public OrganizationAddress()
        {
            IsActive = true;
            IsSuspended = false;
        }
        public long OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }
        public long AddressId { get; set; }
        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }
        public bool IsActive { get; set; }
        public bool IsSuspended { get; set; }
    }
}
