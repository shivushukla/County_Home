using Core.Application;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Application.Impl
{
    public class Settings : ISettings
    {
        public static Settings Default { get; set; }
        public Settings(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public string ConnectionString { get; }
        public dynamic LogConfig { get; private set; }
        public string CorsUrl { get; private set; }
    }
}
