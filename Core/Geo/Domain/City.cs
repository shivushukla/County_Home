using Core.Application.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Geo.Domain
{
    public class City: DomainBase
    {
        public City()
        {
            CityZips = new List<CityZip>();
        }
        public string Name  { get; set; }
        public string ShortName { get; set; }
        public long StateId { get; set; }
        [ForeignKey("StateId")]
        public virtual State State { get; set; }
        public virtual ICollection<CityZip> CityZips { get; set; }
    }
}
