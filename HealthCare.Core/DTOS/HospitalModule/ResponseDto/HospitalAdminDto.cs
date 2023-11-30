using HealthCare.Core.Models.HospitalModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.HospitalModule.ResponseDto
{
    public class HospitalAdminDto
    {

        public string UserName { get; set; }
        public string NationalId { get; set; }
        public string Email { get; set; }
        public string ImagePath { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }
    }
}
