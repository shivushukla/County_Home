using Microsoft.EntityFrameworkCore;
using System.Linq;
using Core.Organizations.Domain;
using Infrastructure.ORM;
using Core.Organizations;
using Infrastructure.Application.Impl;

namespace Infrastructure.Organizations.Impl
{
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public Organization GetOrganization(long id)
        {
            return OrganizationTable.Where(x => x.Id == id).FirstOrDefault();
        }

        public new IQueryable<Organization> List => OrganizationTable;

        private IQueryable<Organization> OrganizationTable
        {
            get
            {
                return DbSet.Include(x => x.OrganizationAddresses).ThenInclude(pv => pv.Address).ThenInclude(x => x.State).
                    Include(x => x.OrganizationAddresses).ThenInclude(pv => pv.Address).ThenInclude(x => x.City).
                    Include(x => x.OrganizationAddresses).ThenInclude(pv => pv.Address).ThenInclude(x => x.Zip).
                    Include(x => x.OrganizationAddresses).ThenInclude(pv => pv.Address).ThenInclude(x => x.Country).
                    Include(x => x.OrganizationRoleUsers).ThenInclude(pv => pv.UserLogin).ThenInclude(x => x.Person).
                    Include(x => x.OrganizationType).ThenInclude(y => y.LookupType).
                    AsQueryable();
            }
        }
    }
}
