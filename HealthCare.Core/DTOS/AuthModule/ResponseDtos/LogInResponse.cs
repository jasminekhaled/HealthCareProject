using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.AuthModule.ResponseDtos
{
    public class LogInResponse
    {
        public string NationalId { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }

        [MinLength(length: 10)]
        [MaxLength(length: 100)]
        public string UserName { get; set; }
        public string ImagePath { get; set; }

        [MinLength(length: 15)]
        [MaxLength(length: 100)]
        public string Role { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }



    }

}
