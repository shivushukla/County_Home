using Core.Application.Domain;
using Core.Organizations.Domain;
using Core.Users.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.ORM
{
    public static class DataEnsureSeeded
    {
        public static void EnsureSeeded(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LookupType>().HasData(
            new LookupType { Id = 1, Name = "Organization Type", Alias = "Organization Type", IsDeleted = false });

            modelBuilder.Entity<Lookup>().HasData(
            new Lookup { Id = 1, LookupTypeId = 1, Name = "Super Admin", Alias = "Super Admin", RelativeOrder = 1, IsDeleted = false, },
            new Lookup { Id = 2, LookupTypeId = 1, Name = "Admin", Alias = "Admin", RelativeOrder = 2 , IsDeleted = false },
            new Lookup { Id = 3, LookupTypeId = 1, Name = "User", Alias = "User", RelativeOrder = 3, IsDeleted = false });

            modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Super Admin", Alias = "Super Admin", Description = null, OrganizationTypeId = 1, IsDeleted = false },
            new Role { Id = 2, Name = "Admin", Alias = "Admin", Description = null, OrganizationTypeId = 2, IsDeleted = false },
            new Role { Id = 3, Name = "User", Alias = "User", Description = null, OrganizationTypeId = 3, IsDeleted = false });

            modelBuilder.Entity<Organization>().HasData(
            new Organization { Id = 1, Name = "Super Admin", About = null, Website = null, Email = null, OrganizationTypeId = 1, IsActive = true, IsSuspended = false, IsDeleted = false, Createdby = 1, CreatedOn = DateTime.Now, Modifiedby = 0, ModifiedOn = null },
            new Organization { Id = 2, Name = "Admin", About = null, Website = null, Email = null, OrganizationTypeId = 2, IsActive = true, IsSuspended = false, IsDeleted = false, Createdby = 1, CreatedOn = DateTime.Now, Modifiedby = 0, ModifiedOn = null },
            new Organization { Id = 3, Name = "User", About = null, Website = null, Email = null, OrganizationTypeId = 3, IsActive = true, IsSuspended = false, IsDeleted = false, Createdby = 1, CreatedOn = DateTime.Now, Modifiedby = 0, ModifiedOn = null });

            modelBuilder.Entity<Person>().HasData(
            new Person { Id = 1, Salutation = null, FirstName = "Super", LastName = "Admin", MiddleName = null,  Email = null, IsActive = true, IsSuspended = false, IsDeleted = false },
            new Person { Id = 2, Salutation = null, FirstName = "Admin", LastName = "", MiddleName = null,  Email = null, IsActive = true, IsSuspended = false, IsDeleted = false },
            new Person { Id = 3, Salutation = null, FirstName = "User", LastName = "", MiddleName = null,  Email = null, IsActive = true, IsSuspended = false, IsDeleted = false });

            modelBuilder.Entity<UserLogin>().HasData(
            new UserLogin { Id = 1, UserName = "Superadmin", Password = "74wviHBlpiwSiWaq1v/cIsYKTvDolmri", Salt = "OuJxBhTqofAZYc73SmoMdICL/wvU2f6d", IsLocked = false, LoginAttemptCount = 0, IsDeleted = false, LastLogginDate = null, ResetToken = null, ResetTokenIssueDateTime = null, IsApproved = true, ApprovedBy = 1, ApprovedDateTime = DateTime.Now },
            new UserLogin { Id = 2, UserName = "Admn", Password = "74wviHBlpiwSiWaq1v/cIsYKTvDolmri", Salt = "OuJxBhTqofAZYc73SmoMdICL/wvU2f6d", IsLocked = false, LoginAttemptCount = 0, IsDeleted = false, LastLogginDate = null, ResetToken = null, ResetTokenIssueDateTime = null, IsApproved = true, ApprovedBy = 1, ApprovedDateTime = DateTime.Now },
            new UserLogin { Id = 3, UserName = "User", Password = "74wviHBlpiwSiWaq1v/cIsYKTvDolmri", Salt = "OuJxBhTqofAZYc73SmoMdICL/wvU2f6d", IsLocked = false, LoginAttemptCount = 0, IsDeleted = false, LastLogginDate = null, ResetToken = null, ResetTokenIssueDateTime = null, IsApproved = true, ApprovedBy = 1, ApprovedDateTime = DateTime.Now });

            modelBuilder.Entity<OrganizationRoleUser>().HasData(
            new OrganizationRoleUser { Id = 1, UserId = 1, RoleId = 1, OrganizationId = 1, IsActive = true, IsDeleted = false },
            new OrganizationRoleUser { Id = 2, UserId = 2, RoleId = 2, OrganizationId = 2, IsActive = true, IsDeleted = false },
            new OrganizationRoleUser { Id = 3, UserId = 3, RoleId = 3, OrganizationId = 3, IsActive = true, IsDeleted = false });

        }
    }
}
