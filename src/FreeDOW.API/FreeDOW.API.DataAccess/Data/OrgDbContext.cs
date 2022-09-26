using FreeDOW.API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FreeDOW.API.DataAccess.Data
{
    public class OrgDbContext : DbContext
    {
        public DbSet<Employee> employees { get; set; }
        public DbSet<EmpSettingValue> empSettingValues { get; set; }
        public DbSet<Equipment> equipments { get; set; }
        public DbSet<Organization> organizations { get; set; }
        public DbSet<OrgSettingValue> orgSettingValues { get; set; }
        public DbSet<OrgStruct> orgStructs { get; set; }
        public DbSet<RegUserEntry> regUserEntries { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<Settings> settings { get; set; }
        
        public OrgDbContext()
        {

        }
        public OrgDbContext(DbContextOptions<OrgDbContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Port=5433;Database=FreeDOW;UserId=owmanage;Password=owmanage");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organization>()
                .ToTable("Organizations")
                .HasKey(o => o.Id );
            
            modelBuilder.Entity<Role>()
                .ToTable("Roles")
                .HasKey(o =>  o.Id );
            
            modelBuilder.Entity<Settings>()
                .ToTable("Settings")
                .HasKey(o =>o.Id);
            
            modelBuilder.Entity<Equipment>()
                .ToTable("Equipments")
                .HasKey(o => o.Id );
            modelBuilder.Entity<Equipment>()
                .HasOne(k => k.Organization)
                .WithMany(e => e.Equipments)
                .HasForeignKey(k => k.OrganizationId);
            
            modelBuilder.Entity<OrgStruct>()
                .ToTable("OrgStruct")
                .HasKey(o => o.Id);
            modelBuilder.Entity<OrgStruct>()
                .HasOne(o => o.Organization)
                .WithMany(o => o.OrgStructs)
                .HasForeignKey(o => o.OrganizationId);
            
            modelBuilder.Entity<Employee>()
                .ToTable("Employees")
                .HasKey(e => e.Id);
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.OrgStruct)
                .WithMany(o => o.Employees)
                .HasForeignKey(e => e.OrgStructId);
            
            modelBuilder.Entity<EmpSettingValue>()
                .ToTable("EmpSettingsValue");
            modelBuilder.Entity<EmpSettingValue>()
                .HasOne(e => e.Settings)
                .WithMany()
                .HasForeignKey(e => e.SettingId);
            modelBuilder.Entity<EmpSettingValue>()
                .HasOne(e => e.Employee)
                .WithMany()
                .HasForeignKey(e => e.EmployeeId);
            
            modelBuilder.Entity<OrgSettingValue>()
                .ToTable("OrgSettingValues");
            modelBuilder.Entity<OrgSettingValue>()
                .HasOne(o => o.Settings)
                .WithMany()
                .HasForeignKey(o => o.SettingId);
            modelBuilder.Entity<OrgSettingValue>()
                .HasOne(o => o.OrgStruct)
                .WithMany()
                .HasForeignKey(o => o.OrgStructId);

            modelBuilder.Entity<EmployeeRole>()
                .ToTable("EmployeeRoles")
                .HasKey(er => new { er.EmployeeID, er.RoleId });
            modelBuilder.Entity<EmployeeRole>()
                .HasOne(er => er.Employee)
                .WithMany(e => e.EmployeeRoles)
                .HasForeignKey(er => er.EmployeeID);
            modelBuilder.Entity<EmployeeRole>()
                .HasOne(er => er.Role)
                .WithMany(e=>e.EmployeeRoles)
                .HasForeignKey(er=>er.RoleId);

            modelBuilder.Entity<Role>()
                .HasData(new Role
                {
                    Id = Guid.Parse("B074FD1F-4739-4669-BF4A-5B3380A8962B"),
                    Name = "Root",
                    Description = "System Addministrator(Admin)"
                }, new Role
                {
                    Id = Guid.Parse("9963C115-8690-42d4-AF89-98B1A155759A"),
                    Name = "OrgRoot",
                    Description = "Organization scheme owner (local organization Admin)"
                }, new Role
                {
                    Id = Guid.Parse("3DC40081-9DB2-44b4-95CC-C85DBA9DFC52"),
                    Name = "OrgStructAdmin",
                    Description = "has rights to manage organization structure where it belongs to"
                }, new Role
                {
                    Id = Guid.Parse("9EB370B3-5CA5-4ce9-822C-ABF66E8AA4E2"),
                    Name = "Employee",
                    Description = "regular employee"
                });
          
        }

    }

    
}
