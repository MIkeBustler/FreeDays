using FreeDOW.API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FreeDOW.API.DataAccess.Data
{
    public class AppDb : DbContext
    {
        public DbSet<Employee> employees { get; set; }
        public DbSet<EmpSettingValue> empSettingValues { get; set; }
        public DbSet<Equipment> equipments { get; set; }
        public DbSet<FreeDay> freeDays { get; set; }
        public DbSet<Organization> organizations { get; set; }
        public DbSet<OrgSettingValue> orgSettingValues { get; set; }
        public DbSet<OrgStruct> orgStructs { get; set; }
        public DbSet<OverWork> overWorks { get; set; }
        public DbSet<RegUserEntry> regUserEntries { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<Settings> settings { get; set; }


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
            modelBuilder.Entity<OverWork>()
                .ToTable("OverWorks")
                .HasKey(o => o.Id);
            modelBuilder.Entity<OverWork>()
                .HasOne(o => o.Employee)
                .WithMany()
                .HasForeignKey(o => o.EmployeeId);
            modelBuilder.Entity<OverWork>()
                .HasOne(o => o.Equipment)
                .WithMany()
                .HasForeignKey(o => o.EquipmentId);
            modelBuilder.Entity<FreeDay>()
                .ToTable("FreeDays")
                .HasKey(f => f.Id);
            modelBuilder.Entity<FreeDay>()
                .HasOne(f => f.Employee)
                .WithMany()
                .HasForeignKey(f => f.EmployeeId);
            modelBuilder.Entity<OWFD>()
                .ToTable("OWFDS");
            modelBuilder.Entity<OWFD>()
                .HasOne(o => o.OverWork)
                .WithMany()
                .HasForeignKey(o => o.OverWorkId);
            modelBuilder.Entity<OWFD>()
                .HasOne(o => o.FreeDay)
                .WithMany()
                .HasForeignKey(o => o.FreeDayId);
        }

        public class AppDbClassFactory : IDesignTimeDbContextFactory<AppDb>
        {
            public AppDbClassFactory()
            {

            }
    
            public AppDb CreateDbContext(string[] args)
            {
                var OptionsBuilder = new DbContextOptionsBuilder<AppDb>();
                OptionsBuilder.UseNpgsql();
                return new AppDb();
            }

        }
    }

    
}
