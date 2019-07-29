using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Application.Domain
{
    public class DomainBase
    {
        public virtual long Id { get; set; }
        public bool IsDeleted { get; set; }
        [NotMapped]
        public bool IsNew { get; set; }
        public long Createdby { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long Modifiedby { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
