using HealthCare.Core.IRepositories.DoctorModule;
using HealthCare.Core.Models.DoctorModule;
using HealthCare.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Repositories.DoctorModule
{
    public class HospitalDoctorRepository : BaseRepository<HospitalDoctor>, IHospitalDoctorRepository
    {

        public HospitalDoctorRepository(ApplicationDbContext context) : base(context)
        {
        }


    }

}
