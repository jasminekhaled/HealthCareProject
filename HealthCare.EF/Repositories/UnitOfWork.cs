using HealthCare.Core.IRepositories;
using HealthCare.Core.IRepositories.AppointmentModule;
using HealthCare.Core.IRepositories.AuthModule;
using HealthCare.Core.IRepositories.BandModule;
using HealthCare.Core.IRepositories.DoctorModule;
using HealthCare.Core.IRepositories.HospitalModule;
using HealthCare.Core.IRepositories.PatientModule;
using HealthCare.EF.Context;
using HealthCare.EF.Repositories.AppointmentRepositories;
using HealthCare.EF.Repositories.AuthModule;
using HealthCare.EF.Repositories.AuthRepositories;
using HealthCare.EF.Repositories.BandRepostiories;
using HealthCare.EF.Repositories.DoctorModule;
using HealthCare.EF.Repositories.HospitalModule;
using HealthCare.EF.Repositories.HospitalRepositories;
using HealthCare.EF.Repositories.PatientRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IUserRepository UserRepository { get; set; }

        public IRoleRepository RoleRepository { get; set; }
        public IUserRoleRepository UserRoleRepository { get; set; }

        public IPatientRepository PatientRepository { get; set; }
        public IDoctorRepository DoctorRepository { get; set; }
        public IDoctorSpecializationRepository DoctorSpecializationRepository { get; set; }
        public ISpecializationRepository SpecializationRepository { get; set; }
        public IMedicalHistoryRepository MedicalHistoryRepository { get; set; }
        public IRateDoctorRepository RateDoctorRepository { get; set; }
        public IHospitalRepository HospitalRepository { get; set; }
        public IAdminOfHospitalRepository AdminOfHospitalRepository { get; set; }
        public IHospitalGovernorateRepository HospitalGovernorateRepository { get; set; }
        public IGovernorateRepository GovernorateRepository { get; set; }
        public IHospitalDoctorRepository HospitalDoctorRepository { get; set; }
        public IHospitalAdminRepository HospitalAdminRepository { get; set; }
        public IClinicLabRepository ClinicLabRepository { get; }
        public IClinicLabDoctorRepository ClinicLabDoctorRepository { get; set; }
        public IReservationRepository ReservationRepository { get; set; }
        public IAppointmentRepository AppointmentRepository { get; set; }
        public IBandRepository BandRepository { get; set; }
        public ICurrentStateRepository CurrentStateRepository { get; set; }
        public ICivilRegestrationRepository CivilRegestrationRepository { get; set; }
        public IRefreshTokenRepository RefreshTokenRepository { get; }
        public IUploadedFileRepository UploadedFileRepository { get; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            RoleRepository = new RoleRepository(context);
            UserRepository = new UserRepository(context);
            UserRoleRepository = new UserRoleRepository(context);
            PatientRepository = new PatientRepository(context);
            DoctorRepository = new DoctorRepository(context);
            DoctorSpecializationRepository = new DoctorSpecializationRepository(context);
            SpecializationRepository = new SpecializationRepository(context);
            HospitalRepository = new HospitalRepository(context);
            RateDoctorRepository = new RateDoctorRepository(context);
            AdminOfHospitalRepository = new AdminOfHospitalRepository(context);
            ClinicLabRepository = new ClinicLabRepository(context);
            HospitalDoctorRepository = new HospitalDoctorRepository(context);
            HospitalAdminRepository = new HospitalAdminRepository(context);
            ClinicLabDoctorRepository = new ClinicLabDoctorRepository(context);
            AppointmentRepository = new AppointmentRepository(context);
            ReservationRepository = new ReservationRepository(context);
            BandRepository = new BandRepository(context);
            CurrentStateRepository = new CurrentStateRepository(context);
            MedicalHistoryRepository = new MedicalHistoryRepository(context);
            CivilRegestrationRepository = new CivilRegestrationRepository(context);
            RefreshTokenRepository = new RefreshTokenRepository(context);
            UploadedFileRepository = new UploadedFileRepository(context);
            GovernorateRepository = new GovernorateRepository(context);
            HospitalGovernorateRepository = new HospitalGovernorateRepository(context);
        }


        public Task<int> CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }


    }

}
