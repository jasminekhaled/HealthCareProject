﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Core.IRepositories.AppointmentModule;
using HealthCare.Core.IRepositories.AuthModule;
using HealthCare.Core.IRepositories.BandModule;
using HealthCare.Core.IRepositories.ClinicModule;
using HealthCare.Core.IRepositories.DoctorModule;
using HealthCare.Core.IRepositories.HospitalModule;
using HealthCare.Core.IRepositories.PatientModule;

namespace HealthCare.Core.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserRepository UserRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public IUserRoleRepository UserRoleRepository { get; }
        public IPatientRepository PatientRepository { get; }
        public IMedicalHistoryRepository MedicalHistoryRepository { get; }
        public IMedicalHistoryFileRepository MedicalHistoryFileRepository { get; }
        public IHospitalRepository HospitalRepository { get; }
        public IHospitalAdminRepository HospitalAdminRepository { get; }
        public IAdminOfHospitalRepository AdminOfHospitalRepository { get; }
        public IHospitalGovernorateRepository HospitalGovernorateRepository { get; }
        public IGovernorateRepository GovernorateRepository { get; }
        public IDoctorRepository DoctorRepository { get; }
        public IDoctorSpecializationRepository DoctorSpecializationRepository { get; }
        public ISpecializationRepository SpecializationRepository { get; }
        public IRateDoctorRepository RateDoctorRepository { get; }
        public IHospitalDoctorRepository HospitalDoctorRepository { get; }
        public IClinicLabRepository ClinicLabRepository { get; }
        public IClinicLabDoctorRepository ClinicLabDoctorRepository { get; }
        public IClinicReservationRepository ClinicReservationRepository { get; }
        public IClinicAppointmentRepository ClinicAppointmentRepository { get; }
        public ILabReservationRepository LabReservationRepository { get; }
        public IXrayAppointmentRepository XrayAppointmentRepository { get; }
        public IXrayReservationRepository XrayReservationRepository { get; }
        public ILabAppointmentRepository LabAppointmentRepository { get; }
        public IBandRepository BandRepository { get; }
        public ISavedBandRepository SavedBandRepository { get; }
        public ICurrentStateRepository CurrentStateRepository { get; }
        public ICivilRegestrationRepository CivilRegestrationRepository { get; }
        public IRefreshTokenRepository RefreshTokenRepository { get; }
        public IUploadedFileRepository UploadedFileRepository { get; }
        public IXraySpecializationRepository XraySpecializationRepository { get; }
        public IXrayDoctorRepository XrayDoctorRepository { get; }
        public IXrayRepository XrayRepository { get; }
        public ILabRepository LabRepository { get; }
        public ILabDoctorRepository LabDoctorRepository { get; }
        public ILabSpecializationRepository LabSpecializationRepository { get; }
        public ISpecializationsOfLabRepository SpecializationsOfLabRepository { get; }
        public IClinicAppointmentDateRepository ClinicAppointmentDateRepository { get; }
        public IXrayAppointmentDateRepository XrayAppointmentDateRepository { get; }
        public ILabAppointmentDateRepository LabAppointmentDateRepository { get; }
        public IDayRepository DayRepository { get; }
        public IAllReservationsRepository AllReservationsRepository { get; }
        Task<int> CompleteAsync();
        void Dispose();
    }
}
