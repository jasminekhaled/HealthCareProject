using HealthCare.Core.IRepositories.AppointmentModule;
using HealthCare.Core.Models.AppointmentModule;
using HealthCare.Core.Models.BandModule;
using HealthCare.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Repositories.AppointmentRepositories
{
    public class ClinicAppointmentRepository : BaseRepository<ClinicAppointment>, IClinicAppointmentRepository
    {

        public ClinicAppointmentRepository(ApplicationDbContext context) : base(context)
        {
        }


    }

}
