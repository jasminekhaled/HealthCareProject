﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.AppointmentModule.RequestDto
{
    public class AddAppointmentDateRequestDto
    {
        public int DayId { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
    }
}
