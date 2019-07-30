using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Application.Domain
{
    public class Lookup : DomainBase
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Alias { get; set; }

        [Required]
        public long LookupTypeId { get; set; }
        [ForeignKey("LookupTypeId")]
        public virtual LookupType LookupType{get; set;}

        public long RelativeOrder { get; set; }
    }
}
