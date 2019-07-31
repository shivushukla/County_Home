using Core.Application;
using System;

namespace DependencyInjection
{
    public class DependencyProvider : IDependencyProvider
    {
        public IServiceProvider ServiceProvider { get; set; }
        public T GetInstance<T>()
        {
            return (T)ServiceProvider.GetService(typeof(T));
        }
    }
}
