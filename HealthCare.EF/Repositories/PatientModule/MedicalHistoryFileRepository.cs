using HealthCare.Core.IRepositories.PatientModule;
using HealthCare.Core.Models.PatientModule;
using HealthCare.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Repositories.PatientModule
{
    public class MedicalHistoryFileRepository : BaseRepository<MedicalHistoryFile>, IMedicalHistoryFileRepository
    {

        public MedicalHistoryFileRepository(ApplicationDbContext context) : base(context)
        {
        }


    }
    
}
