﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.BandModule
{
    public class CurrentState
    {
        public int Id { get; set; }
        public string? Oxygen { get; set; }
        public string? HeartRate { get; set; }
        public string? Temperature { get; set; }
        public string? BloodPressure { get; set; }
        public string? BloodSugar { get; set; }
        public Band Band { get; set; }
    }
}
