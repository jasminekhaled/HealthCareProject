﻿using HealthCare.Core.IRepositories;
using HealthCare.Core.Models.HospitalModule;
using HealthCare.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Repositories
{
    public class DoctorRepository : BaseRepository<Doctor>, IDoctorRepository
    {

        public DoctorRepository(ApplicationDbContext context) : base(context)
        {
        }


    }
    
}
