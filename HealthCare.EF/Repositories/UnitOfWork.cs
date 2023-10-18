using HealthCare.Core.IRepositories;
using HealthCare.EF.Context;
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
        public IUserRoleRepository UserRoleRepository { get; set; }

        public IUserRepository UserRepository { get; set; }

        public IRoleRepository RoleRepository { get; set; }

        public IPatientRepository PatientRepository { get; set; }

        public IDoctorRepository DoctorRepository { get; set; }
        public IMedicalHistoryRepository MedicalHistoryRepository { get; set; }
        public IHospitalRepository HospitalRepository { get; set; }
        public IHospitalDoctorRepository HospitalDoctorRepository { get; set; }
        public IClinicLabRepository ClinicLabRepository { get; }
        public IClinicLabDoctorRepository ClinicLabDoctorRepository { get; set; }
        public IReservationRepository ReservationRepository { get; set; }
        public IAppointmentRepository AppointmentRepository { get; set; }
        public IBandRepository BandRepository { get; set; }
        public ICurrentStateRepository CurrentStateRepository { get; set; }
        public ICivilRegestrationRepository CivilRegestrationRepository { get; set; }
        public IRefreshTokenRepository RefreshTokenRepository { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            RoleRepository = new RoleRepository(context);
            UserRepository = new UserRepository(context);
            UserRoleRepository = new UserRoleRepository(context);
            PatientRepository = new PatientRepository(context);
            DoctorRepository = new DoctorRepository(context);
            HospitalRepository = new HospitalRepository(context);
            ClinicLabRepository = new ClinicLabRepository(context);
            HospitalDoctorRepository = new HospitalDoctorRepository(context);
            ClinicLabDoctorRepository = new ClinicLabDoctorRepository(context);
            AppointmentRepository = new AppointmentRepository(context);
            ReservationRepository = new ReservationRepository(context);
            BandRepository = new BandRepository(context);
            CurrentStateRepository = new CurrentStateRepository(context);
            MedicalHistoryRepository = new MedicalHistoryRepository(context);
            CivilRegestrationRepository = new CivilRegestrationRepository(context);
            RefreshTokenRepository = new RefreshTokenRepository(context);
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
