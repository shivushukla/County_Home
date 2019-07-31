using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application
{
    public class AppManager
    {
        public static IDependencyProvider DependencyInjection { get; set; }
        public static ISettings Settings { get; set; }
    }
}
