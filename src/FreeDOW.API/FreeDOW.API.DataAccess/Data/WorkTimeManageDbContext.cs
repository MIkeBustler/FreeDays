using FreeDOW.API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeDOW.API.DataAccess.Data
{
    public class WorkTimeManageDbContext : DbContext
    {
        public DbSet<FreeDay> freeDays { get; set; }
        public DbSet<OverWork> overWorks { get; set; }
        public DbSet<Vacation> vacation { get; set; }

        public WorkTimeManageDbContext()
        {

        }

        public WorkTimeManageDbContext(DbContextOptions<WorkTimeManageDbContext> options): base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Port=5433;Database=FreeDOW;UserId=owmanage;Password=owmanage");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OverWork>()
                .ToTable("OverWorks")
                .HasKey(o => o.Id);
            modelBuilder.Entity<OverWork>()
                .HasMany(o => o.FreeDays)
                .WithMany(f => f.OverWorks)
                .UsingEntity<OWFD>(
                    t => t
                        .HasOne(of => of.FreeDay)
                        .WithMany(o => o.OWFDS)
                        .HasForeignKey(o => o.FreeDayId),
                    t => t
                        .HasOne(of=>of.OverWork)
                        .WithMany(f=>f.OWFDS)
                        .HasForeignKey(f=>f.OverWorkId),
                    t=>
                    {
                        t.Property(of => of.TimeTaken).HasDefaultValue(0);
                        t.HasKey(of => new { of.FreeDayId, of.OverWorkId });
                        t.ToTable("OWFDS");
                    }
                 );

            modelBuilder.Entity<FreeDay>()
                .ToTable("FreeDays")
                .HasKey(f => f.Id);
            modelBuilder.Entity<Vacation>()
                .ToTable("Vacations")
                .HasKey(v => v.Id);
        }
    }
}
