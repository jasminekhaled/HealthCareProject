using HealthCare.Core.IRepositories.HospitalModule;
using HealthCare.Core.Models;
using HealthCare.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Repositories.HospitalModule
{
    public class GovernorateRepository : BaseRepository<Governorate>, IGovernorateRepository
    {

        public GovernorateRepository(ApplicationDbContext context) : base(context)
        {
        }


    }
}
