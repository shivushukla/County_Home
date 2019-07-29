using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Application.Domain
{
    public class LookupType : DomainBase
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(10)]
        public string Alias { get; set; }

    }
}
