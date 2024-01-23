﻿using HealthCare.Core.IRepositories.ClinicModule;
using HealthCare.Core.Models.ClinicModule;
using HealthCare.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Repositories.ClinicModule
{
    public class XraySpecializationRepository : BaseRepository<XraySpecialization>, IXraySpecializationRepository
    {

        public XraySpecializationRepository(ApplicationDbContext context) : base(context)
        {
        }


    }
}