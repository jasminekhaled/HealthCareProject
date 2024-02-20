﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.HospitalModule.ResponseDto
{
    public class EditHospitalAdminResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string NationalId { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public int HospitalId { get; set; }
        public string HospitalName { get; set; }
        public string HospitalImagePath { get; set; }

    }
}
