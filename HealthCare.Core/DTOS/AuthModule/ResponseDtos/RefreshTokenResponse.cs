using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.AuthModule.ResponseDtos
{
    public class RefreshTokenResponse
    {
        public string Token { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
        public DateTime ExpireOn { get; set; }
    }
}
