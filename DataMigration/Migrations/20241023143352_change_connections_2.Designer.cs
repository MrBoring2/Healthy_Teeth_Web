﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataMigration.Migrations
{
    [DbContext(typeof(HealthyTeethDbContext))]
    [Migration("20241023143352_change_connections_2")]
    partial class change_connections_2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Entities.Account", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("integer");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("EmployeeId");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("text")
                        .HasComputedColumnSql("trim(\"FirstName\" || ' ' || \"MiddleName\" || ' ' || \"LastName\")", true);

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("character varying(11)");

                    b.Property<int>("SpecializationId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FullName");

                    b.HasIndex("SpecializationId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Entities.EmployeeRefreshToken", b =>
                {
                    b.Property<int>("EmployeeId")
                        .HasColumnType("integer");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("RefreshTokenExpiryDate")
                        .HasColumnType("date");

                    b.HasKey("EmployeeId");

                    b.ToTable("EmployeeRefreshTokens");
                });

            modelBuilder.Entity("Entities.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("character varying(40)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("text")
                        .HasComputedColumnSql("trim(\"FirstName\" || ' ' || \"MiddleName\" || ' ' || \"LastName\")", true);

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("MedicalPolicy")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("PassportCode")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)");

                    b.Property<string>("PassportNumber")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("character varying(4)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("character varying(11)");

                    b.HasKey("Id");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Entities.Schedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Cabinet")
                        .HasColumnType("integer");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("TimeFrom")
                        .HasColumnType("interval");

                    b.Property<TimeSpan>("TimeTo")
                        .HasColumnType("interval");

                    b.Property<int>("WeekdayId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("WeekdayId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("Entities.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<int>("SpecializationId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("character varying(80)");

                    b.HasKey("Id");

                    b.HasIndex("SpecializationId");

                    b.HasIndex("Title");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("Entities.ServiceToVisit", b =>
                {
                    b.Property<int>("ServiceId")
                        .HasColumnType("integer");

                    b.Property<int>("VisitId")
                        .HasColumnType("integer");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.HasKey("ServiceId", "VisitId");

                    b.HasIndex("VisitId");

                    b.ToTable("ServiceToVisits");
                });

            modelBuilder.Entity("Entities.Specialization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Specializations");
                });

            modelBuilder.Entity("Entities.Visit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("EmployeeId")
                        .HasColumnType("integer");

                    b.Property<int>("PatientId")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("VisirtTime")
                        .HasColumnType("interval");

                    b.Property<DateTime>("VisitDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("VisitDiagnos")
                        .HasColumnType("text");

                    b.Property<string>("VisitObjectively")
                        .HasColumnType("text");

                    b.Property<string>("VisitPurpose")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("VisitStatusId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("PatientId");

                    b.HasIndex("VisirtTime");

                    b.HasIndex("VisitDate");

                    b.HasIndex("VisitStatusId");

                    b.ToTable("Visits");
                });

            modelBuilder.Entity("Entities.VisitStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("VisitStatuses");
                });

            modelBuilder.Entity("Entities.Weekday", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("Id");

                    b.ToTable("Weekdays");
                });

            modelBuilder.Entity("Entities.Account", b =>
                {
                    b.HasOne("Entities.Employee", "Employee")
                        .WithOne("Account")
                        .HasForeignKey("Entities.Account", "EmployeeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_Account_Employee");

                    b.HasOne("Entities.Role", "Role")
                        .WithMany("Accounts")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_Account_Role");

                    b.Navigation("Employee");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Entities.Employee", b =>
                {
                    b.HasOne("Entities.Specialization", "Specialization")
                        .WithMany("Employees")
                        .HasForeignKey("SpecializationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_Employee_Specialization");

                    b.Navigation("Specialization");
                });

            modelBuilder.Entity("Entities.EmployeeRefreshToken", b =>
                {
                    b.HasOne("Entities.Account", "Account")
                        .WithOne("EmployeeRefreshToken")
                        .HasForeignKey("Entities.EmployeeRefreshToken", "EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Employee_RefreshToken");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Entities.Schedule", b =>
                {
                    b.HasOne("Entities.Employee", "Employee")
                        .WithMany("Schedules")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Schedule_Employee");

                    b.HasOne("Entities.Weekday", "Weekday")
                        .WithMany("Schedules")
                        .HasForeignKey("WeekdayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Schedule_Weekday");

                    b.Navigation("Employee");

                    b.Navigation("Weekday");
                });

            modelBuilder.Entity("Entities.Service", b =>
                {
                    b.HasOne("Entities.Specialization", "Specialization")
                        .WithMany("Services")
                        .HasForeignKey("SpecializationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_Service_Specialization");

                    b.Navigation("Specialization");
                });

            modelBuilder.Entity("Entities.ServiceToVisit", b =>
                {
                    b.HasOne("Entities.Service", "Service")
                        .WithMany("ServiceToVisits")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Entities.Visit", "Visit")
                        .WithMany("ServiceToVisits")
                        .HasForeignKey("VisitId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Service");

                    b.Navigation("Visit");
                });

            modelBuilder.Entity("Entities.Visit", b =>
                {
                    b.HasOne("Entities.Employee", "Employee")
                        .WithMany("Visits")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("FK_Visit_Employee");

                    b.HasOne("Entities.Patient", "Patient")
                        .WithMany("Visits")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("FK_Visit_Patient");

                    b.HasOne("Entities.VisitStatus", "VisitStatus")
                        .WithMany("Visits")
                        .HasForeignKey("VisitStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Patient");

                    b.Navigation("VisitStatus");
                });

            modelBuilder.Entity("Entities.Account", b =>
                {
                    b.Navigation("EmployeeRefreshToken")
                        .IsRequired();
                });

            modelBuilder.Entity("Entities.Employee", b =>
                {
                    b.Navigation("Account");

                    b.Navigation("Schedules");

                    b.Navigation("Visits");
                });

            modelBuilder.Entity("Entities.Patient", b =>
                {
                    b.Navigation("Visits");
                });

            modelBuilder.Entity("Entities.Role", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("Entities.Service", b =>
                {
                    b.Navigation("ServiceToVisits");
                });

            modelBuilder.Entity("Entities.Specialization", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("Services");
                });

            modelBuilder.Entity("Entities.Visit", b =>
                {
                    b.Navigation("ServiceToVisits");
                });

            modelBuilder.Entity("Entities.VisitStatus", b =>
                {
                    b.Navigation("Visits");
                });

            modelBuilder.Entity("Entities.Weekday", b =>
                {
                    b.Navigation("Schedules");
                });
#pragma warning restore 612, 618
        }
    }
}
