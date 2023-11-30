﻿// <auto-generated />
using System;
using HealthCare.EF.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HealthCare.EF.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231129173456_hospitalAdminupdate")]
    partial class hospitalAdminupdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.23")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("HealthCare.Core.Models.AppointmentModule.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ClinicLabId")
                        .HasColumnType("int");

                    b.Property<string>("Day")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("Time")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ClinicLabId");

                    b.HasIndex("DoctorId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("HealthCare.Core.Models.AppointmentModule.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AppointmentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AppointmentId");

                    b.HasIndex("PatientId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("HealthCare.Core.Models.AuthModule.CivilRegestration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NationalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CivilRegestrations");
                });

            modelBuilder.Entity("HealthCare.Core.Models.AuthModule.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiresOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("userId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("HealthCare.Core.Models.AuthModule.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("HealthCare.Core.Models.AuthModule.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NationalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PassWord")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("TableId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("VerificationCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HealthCare.Core.Models.AuthModule.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("HealthCare.Core.Models.BandModule.Band", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CurrentStateId")
                        .HasColumnType("int");

                    b.Property<int>("HospitalId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CurrentStateId");

                    b.HasIndex("HospitalId");

                    b.HasIndex("PatientId");

                    b.ToTable("Bands");
                });

            modelBuilder.Entity("HealthCare.Core.Models.BandModule.CurrentState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BodyTemperature")
                        .HasColumnType("int");

                    b.Property<int>("HeartBeat")
                        .HasColumnType("int");

                    b.Property<int>("OxygenLevel")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("CurrentStates");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.AdminOfHospital", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("HospitalAdminId")
                        .HasColumnType("int");

                    b.Property<int>("HospitalId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HospitalAdminId")
                        .IsUnique();

                    b.HasIndex("HospitalId");

                    b.ToTable("AdminOfHospitals");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.ClinicLab", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ClinicLabs");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.ClinicLabDoctor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ClinicLabId")
                        .HasColumnType("int");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClinicLabId");

                    b.HasIndex("DoctorId");

                    b.ToTable("ClinicLabDoctors");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.Doctor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NationalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PassWord")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Rate")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.Governorate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Governorates");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.Hospital", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<int>("UploadedFileId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UploadedFileId")
                        .IsUnique();

                    b.ToTable("Hospitals");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.HospitalAdmin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NationalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PassWord")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UploadedFileId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("UploadedFileId")
                        .IsUnique();

                    b.ToTable("HospitalAdmins");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.HospitalClinicLab", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ClinicLabId")
                        .HasColumnType("int");

                    b.Property<int>("HospitalId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClinicLabId")
                        .IsUnique();

                    b.HasIndex("HospitalId");

                    b.ToTable("HospitalClinicLabs");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.HospitalDoctor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.Property<int>("HospitalId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.HasIndex("HospitalId");

                    b.ToTable("HospitalDoctors");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.HospitalGovernorate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("GovernorateId")
                        .HasColumnType("int");

                    b.Property<int>("HospitalId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GovernorateId");

                    b.HasIndex("HospitalId")
                        .IsUnique();

                    b.ToTable("HospitalGovernorates");
                });

            modelBuilder.Entity("HealthCare.Core.Models.PatientModule.MedicalHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PatientId");

                    b.ToTable("MedicalHistories");
                });

            modelBuilder.Entity("HealthCare.Core.Models.PatientModule.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("NationalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PassWord")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("VerificationCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("HealthCare.Core.Models.UploadedFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ContentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StoredFileName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UploadedFiles");
                });

            modelBuilder.Entity("HealthCare.Core.Models.AppointmentModule.Appointment", b =>
                {
                    b.HasOne("HealthCare.Core.Models.HospitalModule.ClinicLab", "ClinicLab")
                        .WithMany()
                        .HasForeignKey("ClinicLabId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthCare.Core.Models.HospitalModule.Doctor", "Doctor")
                        .WithMany()
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClinicLab");

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("HealthCare.Core.Models.AppointmentModule.Reservation", b =>
                {
                    b.HasOne("HealthCare.Core.Models.AppointmentModule.Appointment", "Appointment")
                        .WithMany()
                        .HasForeignKey("AppointmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthCare.Core.Models.PatientModule.Patient", "Patient")
                        .WithMany()
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Appointment");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("HealthCare.Core.Models.AuthModule.RefreshToken", b =>
                {
                    b.HasOne("HealthCare.Core.Models.AuthModule.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HealthCare.Core.Models.AuthModule.UserRole", b =>
                {
                    b.HasOne("HealthCare.Core.Models.AuthModule.Role", "Role")
                        .WithMany("UserRole")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthCare.Core.Models.AuthModule.User", "User")
                        .WithOne("UserRole")
                        .HasForeignKey("HealthCare.Core.Models.AuthModule.UserRole", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HealthCare.Core.Models.BandModule.Band", b =>
                {
                    b.HasOne("HealthCare.Core.Models.BandModule.CurrentState", "CurrentState")
                        .WithMany()
                        .HasForeignKey("CurrentStateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthCare.Core.Models.HospitalModule.Hospital", "Hospital")
                        .WithMany()
                        .HasForeignKey("HospitalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthCare.Core.Models.PatientModule.Patient", "Patient")
                        .WithMany()
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrentState");

                    b.Navigation("Hospital");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.AdminOfHospital", b =>
                {
                    b.HasOne("HealthCare.Core.Models.HospitalModule.HospitalAdmin", "HospitalAdmin")
                        .WithOne("AdminOfHospital")
                        .HasForeignKey("HealthCare.Core.Models.HospitalModule.AdminOfHospital", "HospitalAdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthCare.Core.Models.HospitalModule.Hospital", "Hospital")
                        .WithMany("AdminOfHospitals")
                        .HasForeignKey("HospitalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hospital");

                    b.Navigation("HospitalAdmin");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.ClinicLabDoctor", b =>
                {
                    b.HasOne("HealthCare.Core.Models.HospitalModule.ClinicLab", "ClinicLab")
                        .WithMany("clinicLabDoctors")
                        .HasForeignKey("ClinicLabId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthCare.Core.Models.HospitalModule.Doctor", "Doctor")
                        .WithMany("clinicLabDoctors")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClinicLab");

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.Hospital", b =>
                {
                    b.HasOne("HealthCare.Core.Models.UploadedFile", "UploadedFile")
                        .WithOne("Hospital")
                        .HasForeignKey("HealthCare.Core.Models.HospitalModule.Hospital", "UploadedFileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UploadedFile");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.HospitalAdmin", b =>
                {
                    b.HasOne("HealthCare.Core.Models.UploadedFile", "UploadedFile")
                        .WithOne("HospitalAdmin")
                        .HasForeignKey("HealthCare.Core.Models.HospitalModule.HospitalAdmin", "UploadedFileId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("UploadedFile");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.HospitalClinicLab", b =>
                {
                    b.HasOne("HealthCare.Core.Models.HospitalModule.ClinicLab", "ClinicLab")
                        .WithOne("HospitalClinicLab")
                        .HasForeignKey("HealthCare.Core.Models.HospitalModule.HospitalClinicLab", "ClinicLabId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthCare.Core.Models.HospitalModule.Hospital", "Hospital")
                        .WithMany("HospitalClinicLabs")
                        .HasForeignKey("HospitalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClinicLab");

                    b.Navigation("Hospital");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.HospitalDoctor", b =>
                {
                    b.HasOne("HealthCare.Core.Models.HospitalModule.Doctor", "Doctor")
                        .WithMany("hospitalDoctors")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthCare.Core.Models.HospitalModule.Hospital", "Hospital")
                        .WithMany("hospitalDoctors")
                        .HasForeignKey("HospitalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("Hospital");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.HospitalGovernorate", b =>
                {
                    b.HasOne("HealthCare.Core.Models.HospitalModule.Governorate", "Governorate")
                        .WithMany("HospitalGovernorates")
                        .HasForeignKey("GovernorateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthCare.Core.Models.HospitalModule.Hospital", "Hospital")
                        .WithOne("HospitalGovernorate")
                        .HasForeignKey("HealthCare.Core.Models.HospitalModule.HospitalGovernorate", "HospitalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Governorate");

                    b.Navigation("Hospital");
                });

            modelBuilder.Entity("HealthCare.Core.Models.PatientModule.MedicalHistory", b =>
                {
                    b.HasOne("HealthCare.Core.Models.PatientModule.Patient", "Patient")
                        .WithMany()
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("HealthCare.Core.Models.AuthModule.Role", b =>
                {
                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("HealthCare.Core.Models.AuthModule.User", b =>
                {
                    b.Navigation("RefreshTokens");

                    b.Navigation("UserRole")
                        .IsRequired();
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.ClinicLab", b =>
                {
                    b.Navigation("HospitalClinicLab")
                        .IsRequired();

                    b.Navigation("clinicLabDoctors");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.Doctor", b =>
                {
                    b.Navigation("clinicLabDoctors");

                    b.Navigation("hospitalDoctors");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.Governorate", b =>
                {
                    b.Navigation("HospitalGovernorates");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.Hospital", b =>
                {
                    b.Navigation("AdminOfHospitals");

                    b.Navigation("HospitalClinicLabs");

                    b.Navigation("HospitalGovernorate")
                        .IsRequired();

                    b.Navigation("hospitalDoctors");
                });

            modelBuilder.Entity("HealthCare.Core.Models.HospitalModule.HospitalAdmin", b =>
                {
                    b.Navigation("AdminOfHospital")
                        .IsRequired();
                });

            modelBuilder.Entity("HealthCare.Core.Models.UploadedFile", b =>
                {
                    b.Navigation("Hospital")
                        .IsRequired();

                    b.Navigation("HospitalAdmin")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
