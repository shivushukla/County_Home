using Core.Application.Domain;
using Core.Organizations;
using Core.Organizations.Domain;
using Infrastructure.Organizations.Impl;
using Infrastructure.ORM;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Application.Impl
{
    public class RepositoryProvider
    {
        private static Dictionary<Type, Type> repositoryDictionary = new Dictionary<Type, Type>();
        static RepositoryProvider()
        {
            repositoryDictionary.Add(typeof(Organization), typeof(OrganizationRepository));
            repositoryDictionary.Add(typeof(OrganizationRoleUser), typeof(OrganizationRoleUserRepository));
        }
        public static BaseRepository GetInstance<T>(ApplicationDbContext dbContext) where T : DomainBase
        {
            if (repositoryDictionary.ContainsKey(typeof(T)))
            {
                return (BaseRepository)Activator.CreateInstance(repositoryDictionary[typeof(T)], dbContext);
            }

            return new Repository<T>(dbContext);
        }
    }
}
