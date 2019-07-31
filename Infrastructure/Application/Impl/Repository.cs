using Core.Application;
using Core.Application.Domain;
using Infrastructure.ORM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.Application.Impl
{
    public class Repository<T> : BaseRepository, IRepository<T> where T : DomainBase
    {

        protected DbSet<T> DbSet { get { return DbContext.Set<T>(); } }

        public Repository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public IQueryable<T> List => DbSet.AsQueryable();



        public IQueryable<T> Table
        {
            get
            {
                return DbSet.AsQueryable();
            }
        }

        public IEnumerable<T> Fetch(Expression<Func<T, bool>> expression)
        {
            return DbSet.Where(expression).ToArray();
        }

        public T Get(long id)
        {
            return DbSet.Find(id);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return DbSet.SingleOrDefault(expression);
        }

        public void Save(T entity)
        {
            if (entity.IsNew)
            {
                DbContext.ChangeTracker.TrackGraph(entity, node =>
                {
                    var entry = node.Entry;
                    var childEntity = (DomainBase)entry.Entity;
                    entry.State = !entry.IsKeySet && childEntity.Id <= 0 ? EntityState.Added : EntityState.Modified;
                });

                Insert(entity);
            }
            else
            {
                Update(entity);
            }
            DbContext.SaveChanges();
        }


        public void Insert(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Added;


            (new SaveCascadeHelperForInsert()).SetCascadeOnInsert(entity, DbContext);
        }


        public void Update(T entity)
        {
            var original = DbSet.Find(entity.GetId());

            DbContext.Entry(original).CurrentValues.SetValues(entity);
            DbContext.Entry(original).State = EntityState.Modified;

            (new SaveCascadeHelper()).SetCascadeOnModification(original, entity, DbContext);
        }

        public void Delete(T domain)
        {
            if (DbContext.Entry(domain).State == EntityState.Detached)
            {
                DbSet.Attach(domain);
            }

            DbContext.Remove(domain);
            DbContext.SaveChanges();
        }

        public void Delete(Expression<Func<T, bool>> expression)
        {
            foreach (var entity in Fetch(expression))
            {
                Delete(entity);
            }
        }

        public void Delete(long id)
        {
            Delete(x => x.Id == id);
        }

        public IQueryable<T> IncludeMultiple(params Expression<Func<T, object>>[] includes)
        {
            if (includes != null)
            {
                return includes.Aggregate(Table,
                          (current, include) => current.Include(include));
            }

            return Table;
        }
    }
}
