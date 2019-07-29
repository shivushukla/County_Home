using Core.Application.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Geo.Domain
{
    public class Country : DomainBase
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
