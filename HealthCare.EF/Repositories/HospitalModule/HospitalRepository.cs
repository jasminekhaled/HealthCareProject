﻿using HealthCare.Core.IRepositories.HospitalModule;
using HealthCare.Core.Models.HospitalModule;
using HealthCare.Core.Models.PatientModule;
using HealthCare.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Repositories.HospitalRepositories
{
    public class HospitalRepository : BaseRepository<Hospital>, IHospitalRepository
    {

        public HospitalRepository(ApplicationDbContext context) : base(context)
        {
        }


    }

}
