﻿using HealthCare.Core.IRepositories.AppointmentModule;
using HealthCare.Core.Models.AppointmentModule;
using HealthCare.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Repositories.AppointmentModule
{
    public class AllReservationsRepository : BaseRepository<AllReservations>, IAllReservationsRepository
    {

        public AllReservationsRepository(ApplicationDbContext context) : base(context)
        {
        }


    }
}
