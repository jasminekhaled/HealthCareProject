﻿using HealthCare.Core.IRepositories.HospitalModule;
using HealthCare.Core.Models.BandModule;
using HealthCare.Core.Models.HospitalModule;
using HealthCare.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Repositories.HospitalRepositories
{
    public class ClinicLabRepository : BaseRepository<ClinicLab>, IClinicLabRepository
    {

        public ClinicLabRepository(ApplicationDbContext context) : base(context)
        {
        }


    }

}