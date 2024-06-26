﻿using HealthCare.Core.IRepositories.BandModule;
using HealthCare.Core.Models.BandModule;
using HealthCare.Core.Models.HospitalModule;
using HealthCare.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Repositories.BandRepostiories
{
    public class CurrentStateRepository : BaseRepository<CurrentState>, ICurrentStateRepository
    {

        public CurrentStateRepository(ApplicationDbContext context) : base(context)
        {
        }


    }

}
