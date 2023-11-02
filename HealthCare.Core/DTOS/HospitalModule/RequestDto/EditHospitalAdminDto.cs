﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.HospitalModule.RequestDto
{
    public class EditHospitalAdminDto
    {
        [MinLength(length: 10)]
        [MaxLength(length: 100)]
        public string? UserName { get; set; }
        public string? NationalId { get; set; }
        public IFormFile? Image { get; set; }
    }
}
