using Core.Application.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application
{
    public interface IUnitOfWork
    {
        IRepository<T> Repository<T>() where T : DomainBase;
        void StartTransaction();
        void BeginWork(ISettings settings, IDependencyProvider dependencyProvider);
        void Commit();
        void Rollback();
        void Cleanup();
    }
}
