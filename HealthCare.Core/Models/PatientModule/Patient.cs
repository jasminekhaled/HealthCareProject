﻿using HealthCare.Core.Models.DoctorModule;
using HealthCare.Core.Models.HospitalModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.PatientModule
{
    [Table("Patients")]
    public class Patient : GeneralUser
    {
        public bool IsEmailConfirmed { get; set; }
        [StringLength(11)]
        public string PhoneNumber { get; set; }
        public string? VerificationCode { get; set; }
        public List<RateDoctor> RateDoctor { get; set; }

    }
}
