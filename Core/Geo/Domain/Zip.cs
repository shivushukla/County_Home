using Core.Application.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Geo.Domain
{
    public class Zip : DomainBase
    {
        public long ZipCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public virtual ICollection<CityZip> CityZips { get; set; }
    }
}
