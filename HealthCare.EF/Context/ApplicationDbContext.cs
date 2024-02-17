using HealthCare.Core.Models;
using HealthCare.Core.Models.AppointmentModule;
using HealthCare.Core.Models.AuthModule;
using HealthCare.Core.Models.BandModule;
using HealthCare.Core.Models.ClinicModule;
using HealthCare.Core.Models.DoctorModule;
using HealthCare.Core.Models.HospitalModule;
using HealthCare.Core.Models.PatientModule;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<MedicalHistory> MedicalHistories { get; set; }
        public DbSet<MedicalHistoryFile> MedicalHistoryFiles { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<HospitalGovernorate> HospitalGovernorates { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<HospitalAdmin> HospitalAdmins { get; set; }
        public DbSet<AdminOfHospital> AdminOfHospitals { get; set; }
        public DbSet<DoctorSpecialization> DoctorSpecializations { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<HospitalDoctor> HospitalDoctors { get; set; }
        public DbSet<ClinicLab> ClinicLabs { get; set; }
        public DbSet<ClinicLabDoctor> ClinicLabDoctors { get; set; }
        public DbSet<ClinicAppointment> ClinicAppointments { get; set; }
        public DbSet<ClinicReservation> ClinicReservations { get; set; }
        public DbSet<Band> Bands { get; set; }
        public DbSet<SavedBand> SavedBands { get; set; }
        public DbSet<CurrentState> CurrentStates { get; set; }
        public DbSet<CivilRegestration> CivilRegestrations { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<RateDoctor> RateDoctors { get; set; }
        public DbSet<Xray> Xrays { get; set; }
        public DbSet<XrayDoctor> XrayDoctors { get; set; }
        public DbSet<XraySpecialization> XraySpecializations { get; set; }
        public DbSet<Lab> Labs { get; set; }
        public DbSet<LabDoctor> LabDoctors { get; set; }
        public DbSet<LabSpecialization> LabSpecializations { get; set; }
        public DbSet<SpecializationsOfLab> SpecializationsOfLabs { get; set; }
        public DbSet<XrayAppointment> XrayAppointments { get; set; }
        public DbSet<XrayReservation> XrayReservations { get; set; }
        public DbSet<LabAppointment> LabAppointments { get; set; }
        public DbSet<LabReservation> LabReservations { get; set; }
        public DbSet<LabAppointmentDate> LabAppointmentDates { get; set; }
        public DbSet<ClinicAppointmentDate> ClinicAppointmentDates { get; set; }
        public DbSet<XrayAppointmentDate> XrayAppointmentDates { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<AllReservations> AllReservations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshToken>()
                .HasOne(a => a.User)
                .WithMany(r => r.RefreshTokens)
                .HasForeignKey(b => b.userId);

            modelBuilder.Entity<UploadedFile>()
                .HasOne(a => a.Hospital)
                .WithOne(r => r.UploadedFile)
                .HasForeignKey<Hospital>(b => b.UploadedFileId);

            modelBuilder.Entity<UploadedFile>()
                .HasOne(a => a.Specialization)
                .WithOne(r => r.UploadedFile)
                .HasForeignKey<Specialization>(b => b.UploadedFileId)
                .OnDelete(DeleteBehavior.NoAction); ;

            modelBuilder.Entity<UploadedFile>()
                .HasOne(a => a.XraySpecialization)
                .WithOne(r => r.UploadedFile)
                .HasForeignKey<XraySpecialization>(b => b.UploadedFileId);

            modelBuilder.Entity<UploadedFile>()
                .HasOne(a => a.LabSpecialization)
                .WithOne(r => r.UploadedFile)
                .HasForeignKey<LabSpecialization>(b => b.UploadedFileId);

            modelBuilder.Entity<UploadedFile>()
                .HasOne(a => a.User)
                .WithOne(r => r.UploadedFile)
                .HasForeignKey<User>(b => b.UploadedFileId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UploadedFile>()
                .HasOne(a => a.HospitalAdmin)
                .WithOne(r => r.UploadedFile)
                .HasForeignKey<HospitalAdmin>(b => b.UploadedFileId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UploadedFile>()
                .HasOne(a => a.Doctor)
                .WithOne(r => r.UploadedFile)
                .HasForeignKey<Doctor>(b => b.UploadedFileId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UploadedFile>()
                .HasOne(a => a.Patient)
                .WithOne(r => r.UploadedFile)
                .HasForeignKey<Patient>(b => b.UploadedFileId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ClinicLab>()
                .HasOne(a => a.Hospital)
                .WithMany(r => r.ClinicLabs)
                .HasForeignKey(b => b.HospitalId);

            modelBuilder.Entity<Xray>()
                .HasOne(a => a.Hospital)
                .WithMany(r => r.Xrays)
                .HasForeignKey(b => b.HospitalId);

            modelBuilder.Entity<ClinicLab>()
                .HasOne(a => a.Specialization)
                .WithMany(r => r.ClinicLabs)
                .HasForeignKey(b => b.SpecializationId);

            modelBuilder.Entity<Xray>()
                .HasOne(a => a.XraySpecialization)
                .WithMany(r => r.Xrays)
                .HasForeignKey(b => b.XraySpecializationId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<SpecializationsOfLab>()
                .HasOne(a => a.LabSpecialization)
                .WithMany(r => r.SpecializationsOfLab)
                .HasForeignKey(b => b.LabSpecializationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ClinicReservation>()
               .HasOne(a => a.ClinicAppointment)
               .WithMany(r => r.ClinicReservations)
               .HasForeignKey(b => b.ClinicAppointmentId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<XrayReservation>()
               .HasOne(a => a.XrayAppointment)
               .WithMany(r => r.XrayReservations)
               .HasForeignKey(b => b.XrayAppointmentId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<LabReservation>()
               .HasOne(a => a.LabAppointment)
               .WithMany(r => r.LabReservations)
               .HasForeignKey(b => b.LabAppointmentId)
               .OnDelete(DeleteBehavior.NoAction);

           /* modelBuilder.Entity<ClinicAppointment>()
               .HasOne(a => a.Doctor)
               .WithMany(r => r.ClinicAppointment)
               .HasForeignKey(b => b.DoctorId);

            modelBuilder.Entity<XrayAppointment>()
               .HasOne(a => a.Doctor)
               .WithMany(r => r.XrayAppointment)
               .HasForeignKey(b => b.DoctorId);

            modelBuilder.Entity<LabAppointment>()
               .HasOne(a => a.Doctor)
               .WithMany(r => r.LabAppointment)
               .HasForeignKey(b => b.DoctorId);*/

            modelBuilder.Entity<Day>()
                .HasData(new Day[]
                {
                new Day { Id = 10, DayName = "Saturday" },
                new Day { Id = 11, DayName = "Sunday" },
                new Day { Id = 12, DayName = "Monday" },
                new Day { Id = 13, DayName = "Tuesday" },
                new Day { Id = 14, DayName = "Wednesday" },
                new Day { Id = 15, DayName = "Thursday" },
                new Day { Id = 16, DayName = "Friday" }
                });

        }
    }
}
