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
        public DbSet<Weekday> Weekdays { get; set; }
        public DbSet<EmployeeRefreshToken> EmployeeRefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string connectionString = "Host=localhost;Port=5433;Database=healthy-teeth;Persist Security Info=True;User ID=postgres;Password=postgres";

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
                entity.HasOne(p => p.Employee)
                      .WithOne(p => p.Account)
                      .HasConstraintName("FK_Account_Employee")
                      .HasForeignKey<Account>(p => p.EmployeeId)
                      .IsRequired();
                entity.HasOne(p => p.Role)
                      .WithMany(p => p.Accounts)
                      .HasConstraintName("FK_Account_Role")
                      .HasForeignKey(p => p.RoleId);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.DateOfBirth).HasColumnType("date");

                entity.HasOne(p => p.Specialization)
                      .WithMany(p => p.Employees)
                      .HasConstraintName("FK_Employee_Specialization")
                      .HasForeignKey(p => p.SpecializationId)
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
                      .HasForeignKey<EmployeeRefreshToken>(p => p.EmployeeId);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(p => p.Id);
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.HasOne(p => p.Employee)
                      .WithMany(p => p.Schedules)
                      .HasConstraintName("FK_Schedule_Employee")
                      .HasForeignKey(p => p.EmployeeId);

                entity.HasOne(p => p.Weekday)
                      .WithMany(p => p.Schedules)
                      .HasConstraintName("FK_Schedule_Weekday")
                      .HasForeignKey(p => p.WeekdayId);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.HasOne(p => p.Specialization)
                      .WithMany(p => p.Services)
                      .HasConstraintName("FK_Service_Specialization")
                      .HasForeignKey(p => p.SpecializationId);
            });

            modelBuilder.Entity<Visit>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.HasOne(p => p.Employee)
                      .WithMany(p => p.Visits)
                      .HasConstraintName("FK_Visit_Employee")
                      .HasForeignKey(p => p.EmployeeId);

                entity.HasOne(p => p.Patient)
                      .WithMany(p => p.Visits)
                      .HasConstraintName("FK_Visit_Patient")
                      .HasForeignKey(p => p.PatientId);

                entity.HasMany(p => p.Services)
                      .WithMany(p => p.Visits)
                      .UsingEntity<ServiceToVisit>(
                            j => j
                                                        .HasOne<Service>(p => p.Service)
                                                        .WithMany(p => p.ServiceToVisits)
                                                        .HasForeignKey(p => p.ServiceId),
                            j => j
                                                        .HasOne<Visit>(p => p.Visit)
                                                        .WithMany(p => p.ServiceToVisits)
                                                        .HasForeignKey(p => p.VisitId),
                            j =>
                            {
                                j.HasKey(p => new { p.ServiceId, p.VisitId });
                            });

            });
        }
    }
}
