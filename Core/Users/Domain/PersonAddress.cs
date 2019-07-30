using Core.Application.Domain;
using Core.Geo.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Users.Domain
{
    public class PersonAddress: DomainBase
    {
        [NotMapped]
        public long Id { get; set; }
        public long PersonId { get; set; }
        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }
        public long AddressId { get; set; }
        public virtual Address Address { get; set; }
    }
}
