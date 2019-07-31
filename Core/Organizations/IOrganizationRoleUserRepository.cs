using Core.Application;
using Core.Organizations.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Organizations
{
    public interface IOrganizationRoleUserRepository : IRepository<OrganizationRoleUser>
    {
        OrganizationRoleUser GetOrganizationRoleUser(long id);
        OrganizationRoleUser GetForUser(long userId);
    }
}
