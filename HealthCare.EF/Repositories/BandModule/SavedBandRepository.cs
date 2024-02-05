using HealthCare.Core.IRepositories.BandModule;
using HealthCare.Core.Models.BandModule;
using HealthCare.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Repositories.BandModule
{

    public class SavedBandRepository : BaseRepository<SavedBand>, ISavedBandRepository
    {

        public SavedBandRepository(ApplicationDbContext context) : base(context)
        {
        }


    }
}
