﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.AppointmentModule
{
    public class AppointmentDate
    {
        public int Id { get; set; }
        public int DayId { get; set; }
        public Day Day { get; set; }
        public string FromTime { get; set; } 
        public string ToTime { get; set; } 
        
    }
}
