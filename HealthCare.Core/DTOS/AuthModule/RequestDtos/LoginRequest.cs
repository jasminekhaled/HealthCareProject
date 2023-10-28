using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.AuthModule.RequestDtos
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PassWord { get; set; }
    }
}
