﻿using HealthCare.Core.DTOS.HospitalModule.ResponseDto;
using HealthCare.Core.Models.HospitalModule;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.DoctorModule.ResponseDtos
{
    public class AddDoctorResponseDto
    {
        public int DoctorId { get; set; }
        public string NationalId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public List<SpecializationDto> Specializations { get; set; }
        public int HospitalId { get; set; }
        public string HospitalName { get; set; }
        public string HospitalImagePath { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiration { get; set; } 
    }
}
