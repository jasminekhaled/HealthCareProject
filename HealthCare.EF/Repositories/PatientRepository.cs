using HealthCare.Core.IRepositories;
using HealthCare.Core.Models.AppointmentModule;
using HealthCare.Core.Models.PatientModule;
using HealthCare.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Repositories
{
    public class PatientRepository : BaseRepository<Patient>, IPatientRepository
    {

        public PatientRepository(ApplicationDbContext context) : base(context)
        {
        }


    }
    
}
