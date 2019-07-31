using Core.Application.Attributes;
using Core.Application.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Application.Impl
{
    class SaveCascadeHelperForInsert
    {

        private DbContext _dbContext;
        private PropertyInfo _propertyInfo;

        public void SetCascadeOnInsert<T>(T newEntity, DbContext dbContext)
        {
            _dbContext = dbContext;


            var properties = newEntity.GetType().GetProperties().Where(x => x.CustomAttributes.Any(m => m.AttributeType == typeof(CascadeEntityAttribute))).ToArray();

            try
            {
                foreach (var propertyInfo in properties)
                {
                    UpdateCascadeProperty(newEntity, propertyInfo);
                }
            }
            finally { NullAll(); }
        }

        private void UpdateCascadeProperty<T>(T newEntity, PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;

            var attr =
                propertyInfo.GetCustomAttributes(typeof(CascadeEntityAttribute), true).Single() as
                    CascadeEntityAttribute;

            if (!attr.IsCollection && propertyInfo.PropertyType.BaseType != typeof(DomainBase))
            {
                return;
            }

            var objOnNewEntity = propertyInfo.GetValue(newEntity);

            if (objOnNewEntity == null) return;

            if (attr.IsComposite)
            {
                HandleCompositeEntities(attr, objOnNewEntity);
            }
            else if (attr.IsCollection)
            {
                HandleCollections(objOnNewEntity, propertyInfo);
            }
            else
            {
                HandleGeneralReference(objOnNewEntity, propertyInfo);
            }
        }

        private void NullAll()
        {
            _dbContext = null;
            _propertyInfo = null;
        }

        private void HandleCompositeEntities(CascadeEntityAttribute attr, object objOnNewEntity)
        {

            if (attr.IsCollection)
            {
                var collection = objOnNewEntity == null ? new object[0] : (objOnNewEntity as IEnumerable).Cast<object>().ToArray();
                foreach (var item in collection)
                {
                    _dbContext.Entry(item).State = EntityState.Added;
                }

            }
            else
            {
                _dbContext.Entry(objOnNewEntity).State = EntityState.Added;

            }
        }

        private void HandleCollections(object objOnNewEntity, PropertyInfo propertyInfo)
        {


            var newCollection = objOnNewEntity == null ? new List<DomainBase>() : (objOnNewEntity as IEnumerable).OfType<DomainBase>().ToList();

            dynamic collection = Activator
                .CreateInstance(typeof(List<>)
                    .MakeGenericType(_propertyInfo.PropertyType.GetGenericArguments().First()));

            foreach (var item in newCollection)
            {

                var properties =
                        item.GetType()
                            .GetProperties()
                            .Where(x => x.CustomAttributes.Any(m => m.AttributeType == typeof(CascadeEntityAttribute)))
                            .ToArray();
                foreach (var innerPropertyInfo in properties)
                {
                    UpdateCascadeProperty(item, innerPropertyInfo);
                }



                _dbContext.Entry(item).State = EntityState.Added;
                collection.Add((dynamic)item);
            }

            foreach (var item in newCollection.Where(x => (x as DomainBase).Id < 1 || (x as DomainBase).IsNew).ToArray())
            {
                _dbContext.Entry(item).State = EntityState.Added;
                collection.Add((dynamic)item);
            }


        }

        private void HandleGeneralReference(object objOnNewEntity, PropertyInfo propertyInfo)
        {

            if (objOnNewEntity != null)
            {

                _dbContext.Entry(objOnNewEntity).State = EntityState.Added;
                var properties =
                    objOnNewEntity.GetType()
                        .GetProperties()
                        .Where(x => x.CustomAttributes.Any(m => m.AttributeType == typeof(CascadeEntityAttribute)))
                        .ToArray();
                foreach (var innerPropertyInfo in properties)
                {
                    UpdateCascadeProperty(objOnNewEntity, innerPropertyInfo);
                }

            }
        }
    }
}
