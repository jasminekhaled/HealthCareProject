﻿using HealthCare.Core.IRepositories.AppointmentModule;
using HealthCare.Core.Models.AppointmentModule;
using HealthCare.Core.Models.AuthModule;
using HealthCare.EF.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.EF.Repositories.AppointmentRepositories
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {

        public ReservationRepository(ApplicationDbContext context) : base(context)
        {
        }


    }

}