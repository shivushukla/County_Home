using Core.Application;
using Core.Application.Domain;
using Core.Geo.Domain;
using Core.Organizations.Domain;
using Core.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.ORM
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string _connectionString;
        private readonly IDependencyProvider _dependencyProvider;

        public ApplicationDbContext(ISettings settings, IDependencyProvider dependencyProvider) : this(dependencyProvider, settings.ConnectionString)
        {
            _dependencyProvider = dependencyProvider;
        }

        public ApplicationDbContext(DbContextOptions options, IDependencyProvider dependencyProvider, string connString) : base(options)
        {
            _connectionString = connString;
            _dependencyProvider = dependencyProvider;
        }


        public ApplicationDbContext(IDependencyProvider dependencyProvider, string connString)
        {
            _connectionString = connString;
            _dependencyProvider = dependencyProvider;
        }

        public void InitDb()
        {
            var x = Database.GetPendingMigrations();
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }

        public override int SaveChanges()
        {
            TrackChanges(ChangeTracker);
            HandleSoftDelete(ChangeTracker);
            return base.SaveChanges();
        }

        private void TrackChanges(ChangeTracker changeTracker)
        {
            var entities = changeTracker.Entries().Where(e => e.Entity != null && e.Entity is DomainBase && e.State != EntityState.Unchanged
                                                            && (typeof(DomainBase).IsAssignableFrom(e.Entity.GetType())));
            var clock = _dependencyProvider.GetInstance<IClock>();
            var sessionContext = _dependencyProvider.GetInstance<ISessionContext>();
            if (entities != null)
            {
                foreach (var entry in entities)
                {
                    var entity = entry.Entity;
                    var entityBase = entity as DomainBase;
                    if (entry.State == EntityState.Added)
                    {
                        entityBase.CreatedOn = clock.UtcNow;
                        entityBase.Createdby = sessionContext.UserSession?.UserId ?? 0;
                    }
                    else
                    {
                        entityBase.ModifiedOn = clock.UtcNow;
                        entityBase.Modifiedby = sessionContext.UserSession?.UserId ?? 0;
                    }
                }
            }
        }

        private void HandleSoftDelete(ChangeTracker changeTracker)
        {
            foreach (EntityEntry entry in changeTracker.Entries().Where(w => w.State == EntityState.Deleted))
            {
                entry.Property("IsDeleted").CurrentValue = true;
                entry.State = EntityState.Modified;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CityZip>().HasKey(x => new { x.CityId, x.ZipId });
            modelBuilder.Entity<PersonAddress>().HasKey(x => new { x.PersonId, x.AddressId });
            modelBuilder.Entity<OrganizationAddress>().HasKey(x => new { x.OrganizationId, x.AddressId });

            modelBuilder.Entity<CityZip>().HasOne(bc => bc.City).WithMany(b => b.CityZips).HasForeignKey(bc => bc.CityId);
            modelBuilder.Entity<CityZip>().HasOne(bc => bc.Zip).WithMany(c => c.CityZips).HasForeignKey(bc => bc.ZipId);

            modelBuilder.Entity<OrganizationAddress>().HasOne(bc => bc.Address).WithMany(b => b.OrganizationAddress).HasForeignKey(bc => bc.AddressId);
            modelBuilder.Entity<OrganizationAddress>().HasOne(bc => bc.Organization).WithMany(c => c.OrganizationAddresses).HasForeignKey(bc => bc.OrganizationId);

            modelBuilder.Entity<PersonAddress>().HasOne(bc => bc.Address).WithMany(b => b.PersonAddress).HasForeignKey(bc => bc.AddressId);
            modelBuilder.Entity<PersonAddress>().HasOne(bc => bc.Person).WithMany(c => c.PersonAddresses).HasForeignKey(bc => bc.PersonId);

            modelBuilder.Entity<Organization>().Property(b => b.IsActive).HasDefaultValueSql("1");
            modelBuilder.Entity<Organization>().Property(b => b.IsSuspended).HasDefaultValueSql("0");
            modelBuilder.Entity<Person>().Property(b => b.IsSuspended).HasDefaultValueSql("0");



            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Discriminator column
                modelBuilder.Entity(entityType.ClrType).HasDiscriminator("IsDeleted", typeof(bool)).HasValue(false);
                // Shadow Property       
                modelBuilder.Entity(entityType.ClrType).Property(typeof(bool), "IsDeleted").IsRequired(true);
                modelBuilder.Entity(entityType.ClrType).Property(typeof(bool), "IsDeleted").Metadata.AfterSaveBehavior = PropertySaveBehavior.Save;

                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var body = Expression.Equal(Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(bool) }, parameter, Expression.Constant("IsDeleted")), Expression.Constant(false));

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(Expression.Lambda(body, parameter));
            }


            ConfigureTypesWithNoAutoGeneratedID(modelBuilder, typeof(Lookup), typeof(LookupType), typeof(City), typeof(Country), typeof(State), typeof(Zip), typeof(UserLogin));

            base.OnModelCreating(modelBuilder);
            modelBuilder.EnsureSeeded();
        }

        private void ConfigureTypesWithNoAutoGeneratedID(ModelBuilder modelBuilder, params Type[] types)
        {
            foreach (var type in types)
            {
                modelBuilder.Entity(type).Property("Id").ValueGeneratedNever().HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.None);
            }
        }

        public virtual DbSet<LookupType> LookupType { get; set; }
        public virtual DbSet<Lookup> Lookup { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<State> State { get; set; }
        public virtual DbSet<Zip> Zip { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<CityZip> CityZip { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<PersonAddress> PersonAddress { get; set; }
        public virtual DbSet<UserLogin> UserLogin { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Organization> Organization { get; set; }
        public virtual DbSet<OrganizationAddress> OrganizationAddress { get; set; }
        public virtual DbSet<OrganizationRoleUser> OrganizationRoleUser { get; set; }

    }
}
