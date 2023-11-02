using HealthCare.Core.IRepositories.HospitalModule;
using HealthCare.Core.Models.HospitalModule;
using HealthCare.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Repositories.HospitalModule
{
    public class HospitalClinicLabRepository : BaseRepository<HospitalClinicLab>, IHospitalClinicLabRepository
    {

        public HospitalClinicLabRepository(ApplicationDbContext context) : base(context)
        {
        }


    }
}
