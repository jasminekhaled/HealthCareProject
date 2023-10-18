﻿using HealthCare.Core.IRepositories;
using HealthCare.Core.Models.AuthModule;
using HealthCare.Core.Models.HospitalModule;
using HealthCare.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Repositories
{
    public class CivilRegestrationRepository : BaseRepository<CivilRegestration>, ICivilRegestrationRepository
    {

        public CivilRegestrationRepository(ApplicationDbContext context) : base(context)
        {
        }


    }

}
