using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application
{
    public interface IDependencyProvider
    {
        T GetInstance<T>();
    }
}
