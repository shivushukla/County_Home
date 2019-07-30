using Core.Application.Domain;
using Core.Organizations.Domain;
using Core.Users.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Geo.Domain
{
    public class Address : DomainBase
    {
        public Address(long id, string addressLine, string city, string zipCode, string state, long countryId)
        {
            Id = id;
            IsNew = id < 1;
            Line1 = addressLine;
            City = city;
            State = state;
            Zip = zipCode;
            CountryId = countryId;
        }

        public string Line1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public long CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }

        public virtual ICollection<PersonAddress> PersonAddress { get; set; }
        public IEnumerable<OrganizationAddress> OrganizationAddress { get; set; }
    }
}
