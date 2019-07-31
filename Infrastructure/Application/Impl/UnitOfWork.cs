using Core.Application;
using Core.Application.Domain;
using Infrastructure.ORM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Infrastructure.Application.Impl   
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private Dictionary<string, BaseRepository> _repositories;
        private DbTransaction _transaction;
        private DbConnection _dbConnection;

        internal ApplicationDbContext DbContext { get; private set; }

        public UnitOfWork(ISettings settings, IDependencyProvider dependencyProvider)
        {
            DbContext = new ApplicationDbContext(settings, dependencyProvider);
            _repositories = new Dictionary<string, BaseRepository>();

        }

        public void BeginWork(ISettings settings, IDependencyProvider dependencyProvider)
        {
            if (DbContext == null)
            {
                DbContext = new ApplicationDbContext(settings, dependencyProvider);
                _repositories = new Dictionary<string, BaseRepository>();
            }
        }

        public void Cleanup()
        {
            if (_transaction != null) _transaction.Rollback();
            _transaction = null;

            if (DbContext != null)
            {
                DbContext.Dispose();
                DbContext = null;
            }

            if (_dbConnection != null)
            {
                if (_dbConnection.State == System.Data.ConnectionState.Open)
                    _dbConnection.Close();

                _dbConnection.Dispose();
                _dbConnection = null;
            }

        }

        public void Commit()
        {
            try
            {
                if (_transaction != null)
                {
                    _transaction.Commit();
                    _transaction.Dispose();
                    DbContext.Database.UseTransaction(null);
                }
            }
            finally
            {
                _transaction = null;
            }
        }

        public IRepository<T> Repository<T>() where T : DomainBase
        {
            string typeOfRepo = typeof(T).FullName;
            if (!_repositories.ContainsKey(typeOfRepo))
            {
                _repositories.Add(typeOfRepo, RepositoryProvider.GetInstance<T>(DbContext));
            }
            _repositories.TryGetValue(typeOfRepo, out var data);
            return (Repository<T>)data;
        }

        public void Rollback()
        {
            try
            {
                if (_transaction != null)
                {
                    _transaction.Rollback();
                    _transaction.Dispose();
                    DbContext.Database.UseTransaction(null);
                }
            }
            finally
            {
                _transaction = null;
            }
        }

        public void StartTransaction()
        {
            if (_dbConnection == null)
            {
                _dbConnection = DbContext.Database.GetDbConnection();
            }

            if (_dbConnection.State == System.Data.ConnectionState.Closed)
            {
                _dbConnection.Open();
            }

            if (_transaction == null)
            {
                _transaction = _dbConnection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                DbContext.Database.UseTransaction(_transaction);
            }
        }

        public void Dispose()
        {
            this.Cleanup();
        }
    }
}
