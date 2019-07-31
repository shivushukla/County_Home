using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application
{
    public interface ISettings
    {
        string ConnectionString { get; }

        dynamic LogConfig
        {
            get;
        }
    }
}
