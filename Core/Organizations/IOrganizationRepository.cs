using Core.Application;
using Core.Organizations.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Organizations
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        Organization GetOrganization(long id);
    }
}
