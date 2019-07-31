using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Impl;
using Core.Application;
using Core.Users;
using Core.Users.Impl;
using DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDependencyProvider>(x =>
            {
                var d = new DependencyProvider
                {
                    ServiceProvider = x
                };
                return d;
            });

            DependencyRegistrar.RegisterDependencies(services);
            DependencyRegistrar.RegisterScopedDependencies(services);
            services.AddScoped<ISessionContext, SessionContext>();
            services.AddTransient<ILoginAuthenticationModelValidator, LoginAuthenticationModelValidator>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var serviceProvide = services.BuildServiceProvider();

            AppManager.DependencyInjection = new DependencyProvider
            {
                ServiceProvider = serviceProvide
            };

            ServicesRegistrar.RegisterServices(Settings.Default, serviceProvide.GetService<IDependencyProvider>());

            services.AddCors(setup => setup.AddPolicy("CorsPolicy", builder =>
            {
                builder.WithOrigins(Settings.Default.CorsUrl)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            }));

            services.AddHttpContextAccessor();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(options =>
            {
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
