using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Attributes
{
    public class CascadeEntityAttribute : System.Attribute
    {
        public bool IsComposite { get; set; }

        public bool IsCollection { get; set; }

        public CascadeEntityAttribute(bool isComposite, bool isCollection = false)
        {
            IsCollection = isCollection;
            IsComposite = isComposite;
        }

        public CascadeEntityAttribute()
            : this(false)
        {
        }
    }
}
