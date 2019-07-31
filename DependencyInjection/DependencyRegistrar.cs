using Core.Application;
using Core.Application.Attributes;
using Infrastructure.Application.Impl;
using Infrastructure.ORM;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DependencyInjection
{
    public static class DependencyRegistrar
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            RegisterDefaultImplementations(services);
            var provider = services.BuildServiceProvider();
            RegisterAppManager(provider);
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<ApplicationDbContext>(ServiceLifetime.Scoped);
        }

        public static void RegisterScopedDependencies(IServiceCollection services)
        {
        }

        public static void RegisterAppManager(IServiceProvider provider)
        {

        }
        private static void RegisterDefaultImplementations(IServiceCollection services)
        {
            var publicTypes = typeof(DependencyRegistrar).Assembly.GetReferencedAssemblies()
                .Where(m =>
                {
                    return m.Name.Contains("County");
                }).Select(Assembly.Load).SelectMany(a => a.GetTypes());

            var interfaces = publicTypes.Where(t => t.IsInterface);
            var defaultImplementations = publicTypes.Where(type => Attribute.IsDefined(type, typeof(DefaultImplementationAttribute)));

            foreach (var type in defaultImplementations)
            {
                var attribute = (DefaultImplementationAttribute)type.GetCustomAttributes(typeof(DefaultImplementationAttribute), false).Single();
                Type typeClosure = type;

                attribute.Interface = attribute.Interface ?? interfaces.Single(i => i.Name == "I" + typeClosure.Name);
                services.AddTransient(attribute.Interface, type);

            }

        }
    }
}
