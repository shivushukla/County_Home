using Core.Application.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Geo.Domain
{
    public class CityZip : DomainBase
    {
        [NotMapped]
        public long Id { get; set; }
        public long CityId { get; set; }
        [ForeignKey("CityId")]
        public virtual City City { get; set; }
        public long ZipId { get; set; }
        [ForeignKey("ZipId")]
        public virtual Zip Zip { get; set; }
    }
}
