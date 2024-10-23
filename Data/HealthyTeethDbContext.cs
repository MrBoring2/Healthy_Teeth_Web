using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class HealthyTeethDbContext : DbContext
    {
        public HealthyTeethDbContext()
        {

        }
        public HealthyTeethDbContext(DbContextOptions<HealthyTeethDbContext> options) : base(options)
        {

        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceToVisit> ServiceToVisits { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<VisitStatus> VisitStatuses { get; set; }
        public DbSet<EmployeeRefreshToken> EmployeeRefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //const string connectionString = "Host=localhost;Port=5433;Database=healthy-teeth;Persist Security Info=True;User ID=postgres;Password=postgres";

            //optionsBuilder.UseLazyLoadingProxies()
            //              .LogTo(message => Debug.WriteLine(message))
            //              .EnableSensitiveDataLogging();
            //optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {

                entity.HasKey(p => p.EmployeeId);
                entity.HasIndex(p => p.Login).IsUnique();
                entity.HasOne(p => p.Employee)
                      .WithOne(p => p.Account)
                      .HasConstraintName("FK_Account_Employee")
                      .HasForeignKey<Account>(p => p.EmployeeId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(p => p.Role)
                      .WithMany(p => p.Accounts)
                      .HasConstraintName("FK_Account_Role")
                      .HasForeignKey(p => p.RoleId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.DateOfBirth).HasColumnType("date");
                entity.Property(p => p.FullName).HasComputedColumnSql(@"trim(""FirstName"" || ' ' || ""MiddleName"" || ' ' || ""LastName"")", stored: true);
                entity.HasIndex(p => p.FullName);
                entity.HasOne(p => p.Specialization)
                      .WithMany(p => p.Employees)
                      .HasConstraintName("FK_Employee_Specialization")
                      .HasForeignKey(p => p.SpecializationId)
                      .OnDelete(DeleteBehavior.NoAction)
                      .IsRequired();

            });

            modelBuilder.Entity<EmployeeRefreshToken>(entity =>
            {
                entity.HasKey(p => p.EmployeeId);
                entity.Property(p => p.RefreshTokenExpiryDate).HasColumnType("date");
                entity.Property(p => p.RefreshToken).HasColumnType("text");
                entity.HasOne(p => p.Account)
                      .WithOne(p => p.EmployeeRefreshToken)
                      .HasConstraintName("FK_Employee_RefreshToken")
                      .HasForeignKey<EmployeeRefreshToken>(p => p.EmployeeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.FullName).HasComputedColumnSql(@"trim(""FirstName"" || ' ' || ""MiddleName"" || ' ' || ""LastName"")", stored: true);
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.HasOne(p => p.Employee)
                      .WithMany(p => p.Schedules)
                      .HasConstraintName("FK_Schedule_Employee")
                      .HasForeignKey(p => p.EmployeeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.HasIndex(p => p.Title);
                entity.HasOne(p => p.Specialization)
                      .WithMany(p => p.Services)
                      .HasConstraintName("FK_Service_Specialization")
                      .HasForeignKey(p => p.SpecializationId)
                      .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Visit>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.HasIndex(p => p.VisitDate);
                entity.HasIndex(p => p.VisirtTime);
                entity.HasOne(p => p.Employee)
                      .WithMany(p => p.Visits)
                      .HasConstraintName("FK_Visit_Employee")
                      .HasForeignKey(p => p.EmployeeId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(p => p.Patient)
                      .WithMany(p => p.Visits)
                      .HasConstraintName("FK_Visit_Patient")
                      .HasForeignKey(p => p.PatientId)
                      .OnDelete(DeleteBehavior.NoAction);

                entity.HasMany(p => p.Services)
                      .WithMany(p => p.Visits)
                      .UsingEntity<ServiceToVisit>(
                            j => j
                                                        .HasOne<Service>(p => p.Service)
                                                        .WithMany(p => p.ServiceToVisits)
                                                        .HasForeignKey(p => p.ServiceId)
                                                        .OnDelete(DeleteBehavior.NoAction),
                            j => j
                                                        .HasOne<Visit>(p => p.Visit)
                                                        .WithMany(p => p.ServiceToVisits)
                                                        .HasForeignKey(p => p.VisitId)
                                                        .OnDelete(DeleteBehavior.NoAction),
                            j =>
                            {
                                j.HasKey(p => new { p.ServiceId, p.VisitId });
                            });

            });
        }
    }
}
