using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Attributes
{
    public class DefaultImplementationAttribute : Attribute
    {
        public Type Interface { get; set; }
        public string RegistrationName { get; set; }
        public DefaultImplementationAttribute() : this(null, null)
        {
        }

        public DefaultImplementationAttribute(Type interfaceType) : this(interfaceType, null)
        {
        }

        public DefaultImplementationAttribute(Type interfaceType, string registrationName)
        {
            Interface = interfaceType;
            RegistrationName = registrationName;
        }
    }
}
