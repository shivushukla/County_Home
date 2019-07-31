using Core.Organizations;
using Core.Organizations.Domain;
using Infrastructure.Application.Impl;
using Infrastructure.ORM;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Organizations.Impl
{
    public class OrganizationRoleUserRepository : Repository<OrganizationRoleUser>, IOrganizationRoleUserRepository
    {

        public OrganizationRoleUserRepository(ApplicationDbContext opinionRouteDbContext) : base(opinionRouteDbContext)
        {

        }
        public OrganizationRoleUser GetOrganizationRoleUser(long id)
        {
            return OrganizationRoleUserTable.SingleOrDefault(x => x.Id == id);
        }

        public OrganizationRoleUser GetForUser(long userId)
        {
            return OrganizationRoleUserTable.Where(x => x.UserId == userId).FirstOrDefault();
        }

        public new IQueryable<OrganizationRoleUser> List => OrganizationRoleUserTable;

        private IQueryable<OrganizationRoleUser> OrganizationRoleUserTable
        {
            get
            {

                return DbSet.Include(x => x.Organization).
                    Include(x => x.UserLogin).ThenInclude(pv => pv.Person).
                    Include(x => x.UserLogin).ThenInclude(pv => pv.Person).ThenInclude(x => x.PersonAddresses).ThenInclude(pv => pv.Address).ThenInclude(x => x.Country).
                    Include(x => x.UserLogin).ThenInclude(pv => pv.Person).ThenInclude(x => x.PersonAddresses).ThenInclude(pv => pv.Address).ThenInclude(x => x.City).
                    Include(x => x.UserLogin).ThenInclude(pv => pv.Person).ThenInclude(x => x.PersonAddresses).ThenInclude(pv => pv.Address).ThenInclude(x => x.State).
                    Include(x => x.UserLogin).ThenInclude(pv => pv.Person).ThenInclude(x => x.PersonAddresses).ThenInclude(pv => pv.Address).ThenInclude(x => x.Zip).
                    Include(x => x.Role)
                     .AsQueryable();

            }
        }
    }
}
