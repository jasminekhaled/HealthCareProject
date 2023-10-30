using HealthCare.Core.IRepositories.BandModule;
using HealthCare.Core.Models.AuthModule;
using HealthCare.Core.Models.BandModule;
using HealthCare.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Repositories.BandRepostiories
{
    public class BandRepository : BaseRepository<Band>, IBandRepository
    {

        public BandRepository(ApplicationDbContext context) : base(context)
        {
        }


    }

}
