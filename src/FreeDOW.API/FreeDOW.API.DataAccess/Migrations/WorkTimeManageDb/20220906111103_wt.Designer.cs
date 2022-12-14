// <auto-generated />
using System;
using FreeDOW.API.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FreeDOW.API.DataAccess.Migrations.WorkTimeManageDb
{
    [DbContext(typeof(WorkTimeManageDbContext))]
    [Migration("20220906111103_wt")]
    partial class wt
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FreeDOW.API.Core.Entities.FreeDay", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Confirmed")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DTEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DTStart")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("FreeDays", (string)null);
                });

            modelBuilder.Entity("FreeDOW.API.Core.Entities.OverWork", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Confirmed")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DTEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DTStart")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Desription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid");

                    b.Property<string>("EquipmentDetail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("EquipmentId")
                        .HasColumnType("uuid");

                    b.Property<int>("OverheadAddTime")
                        .HasColumnType("integer");

                    b.Property<int>("Reminder")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("OverWorks", (string)null);
                });

            modelBuilder.Entity("FreeDOW.API.Core.Entities.OWFD", b =>
                {
                    b.Property<Guid>("FreeDayId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("OverWorkId")
                        .HasColumnType("uuid");

                    b.Property<int>("TimeTaken")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.HasKey("FreeDayId", "OverWorkId");

                    b.HasIndex("OverWorkId");

                    b.ToTable("OWFDS", (string)null);
                });

            modelBuilder.Entity("FreeDOW.API.Core.Entities.Vacation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Confirmed")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DTEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DTStart")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("FactDTEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("FactDTStart")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("VacationType")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Vacations", (string)null);
                });

            modelBuilder.Entity("FreeDOW.API.Core.Entities.OWFD", b =>
                {
                    b.HasOne("FreeDOW.API.Core.Entities.FreeDay", "FreeDay")
                        .WithMany("OWFDS")
                        .HasForeignKey("FreeDayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FreeDOW.API.Core.Entities.OverWork", "OverWork")
                        .WithMany("OWFDS")
                        .HasForeignKey("OverWorkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FreeDay");

                    b.Navigation("OverWork");
                });

            modelBuilder.Entity("FreeDOW.API.Core.Entities.FreeDay", b =>
                {
                    b.Navigation("OWFDS");
                });

            modelBuilder.Entity("FreeDOW.API.Core.Entities.OverWork", b =>
                {
                    b.Navigation("OWFDS");
                });
#pragma warning restore 612, 618
        }
    }
}
