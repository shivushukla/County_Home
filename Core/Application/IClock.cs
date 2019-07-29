using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application
{
    public interface IClock
    {
        DateTime UtcNow { get; }
    }
}
