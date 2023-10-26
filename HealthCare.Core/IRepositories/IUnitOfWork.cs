using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserRepository UserRepository { get; }
        public IRoleRepository RoleRepository { get; }
        public IUserRoleRepository UserRoleRepository { get; }
        public IPatientRepository PatientRepository { get; }
        public IMedicalHistoryRepository MedicalHistoryRepository { get; }
        public IHospitalRepository HospitalRepository { get; }
        public IDoctorRepository DoctorRepository { get; }
        public IHospitalDoctorRepository HospitalDoctorRepository { get; }
        public IClinicLabRepository ClinicLabRepository { get; }
        public IClinicLabDoctorRepository ClinicLabDoctorRepository { get; }
        public IReservationRepository ReservationRepository { get; }
        public IAppointmentRepository AppointmentRepository { get; }
        public IBandRepository BandRepository { get; }
        public ICurrentStateRepository CurrentStateRepository { get; }
        public ICivilRegestrationRepository CivilRegestrationRepository { get; }
        public IRefreshTokenRepository RefreshTokenRepository { get; }
        Task<int> CompleteAsync();
        void Dispose();
    }
}
