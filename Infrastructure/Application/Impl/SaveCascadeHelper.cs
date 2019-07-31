﻿using Core.Application.Attributes;
using Core.Application.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Application.Impl
{
    class SaveCascadeHelper
    {
        private DbContext _dbContext;
        private PropertyInfo _propertyInfo;
        private object _originalEntity;

        public void SetCascadeOnModification<T>(T dbEntity, T newEntity, DbContext dbContext)
        {
            _dbContext = dbContext;
            _originalEntity = dbEntity;


            var properties = newEntity.GetType().GetProperties().Where(x => x.CustomAttributes.Any(m => m.AttributeType == typeof(CascadeEntityAttribute))).ToArray();

            try
            {
                foreach (var propertyInfo in properties)
                {
                    UpdateCascadeProperty(dbEntity, newEntity, propertyInfo, _originalEntity);
                }
            }
            finally { NullAll(); }
        }

        private void UpdateCascadeProperty<T>(T dbEntity, T newEntity, PropertyInfo propertyInfo, object originalEntity)
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
            var objOnOldEntity = propertyInfo.GetValue(dbEntity);

            if (objOnNewEntity == objOnOldEntity && objOnNewEntity == null) return;

            if (attr.IsComposite)
            {
                HandleCompositeEntities(attr, objOnOldEntity, objOnNewEntity);
            }
            else if (attr.IsCollection)
            {
                HandleCollections(objOnOldEntity, objOnNewEntity, propertyInfo, originalEntity);
            }
            else
            {
                HandleGeneralReference(objOnOldEntity, objOnNewEntity, propertyInfo);
            }
        }

        private void NullAll()
        {
            _dbContext = null;
            _propertyInfo = null;
            _originalEntity = null;
        }

        private void HandleCompositeEntities(CascadeEntityAttribute attr, object objOnOldEntity, object objOnNewEntity)
        {

            if (attr.IsCollection)
            {
                var collection = objOnNewEntity == null ? new object[0] : (objOnNewEntity as IEnumerable).Cast<object>().ToArray();
                foreach (var item in collection)
                {
                    _dbContext.Entry(item).State = EntityState.Added;
                }
                collection = objOnOldEntity == null ? new object[0] : (objOnOldEntity as IEnumerable).Cast<object>().ToArray();
                foreach (var item in collection)
                {
                    _dbContext.Entry(item).State = EntityState.Modified;
                }
            }
            else
            {
                _dbContext.Entry(objOnOldEntity).State = EntityState.Modified;

            }
        }

        private void HandleCollections(object objOnOldEntity, object objOnNewEntity, PropertyInfo propertyInfo, object originalEntity)
        {

            var inDBCollection = objOnOldEntity == null ? new List<DomainBase>() : (objOnOldEntity as IEnumerable).OfType<DomainBase>().ToList();
            var newCollection = objOnNewEntity == null ? new List<DomainBase>() : (objOnNewEntity as IEnumerable).OfType<DomainBase>().ToList();

            dynamic collection = Activator
                .CreateInstance(typeof(List<>)
                    .MakeGenericType(_propertyInfo.PropertyType.GetGenericArguments().First()));

            foreach (var item in inDBCollection)
            {
                var newObject = newCollection.SingleOrDefault(x => x.Id == (item as DomainBase).Id);
                if (newObject == null)
                {
                    _dbContext.Entry(item).State = EntityState.Deleted;
                    continue;
                }
                var properties =
                        newObject.GetType()
                            .GetProperties()
                            .Where(x => x.CustomAttributes.Any(m => m.AttributeType == typeof(CascadeEntityAttribute)))
                            .ToArray();
                foreach (var innerPropertyInfo in properties)
                {
                    UpdateCascadeProperty(item, newObject, innerPropertyInfo, item);
                }


                _dbContext.Entry(item).CurrentValues.SetValues(newObject);
                _dbContext.Entry(item).State = EntityState.Modified;
                collection.Add((dynamic)item);
            }

            foreach (var item in newCollection.Where(x => (x as DomainBase).Id < 1 || (x as DomainBase).IsNew).ToArray())
            {
                _dbContext.Entry(item).State = EntityState.Added;
                collection.Add((dynamic)item);
            }

            propertyInfo.SetValue(originalEntity, collection);
        }

        private void HandleGeneralReference(object objOnOldEntity, object objOnNewEntity, PropertyInfo propertyInfo)
        {

            if (objOnNewEntity == null)
            {

                _dbContext.Entry(objOnOldEntity).State = EntityState.Deleted;
            }
            else if (objOnOldEntity == null)
            {
                _dbContext.Entry(objOnNewEntity).State = EntityState.Added;
                propertyInfo.SetValue(_originalEntity, objOnNewEntity);
            }
            else
            {
                _dbContext.Entry(objOnOldEntity).CurrentValues.SetValues(objOnNewEntity);
                _dbContext.Entry(objOnOldEntity).State = EntityState.Modified;
            }
            if (objOnNewEntity != null)
            {
                var properties =
                    objOnNewEntity.GetType()
                        .GetProperties()
                        .Where(x => x.CustomAttributes.Any(m => m.AttributeType == typeof(CascadeEntityAttribute)))
                        .ToArray();
                foreach (var innerPropertyInfo in properties)
                {
                    UpdateCascadeProperty(objOnOldEntity, objOnNewEntity, innerPropertyInfo, objOnOldEntity);
                }

            }
        }
    }
}
