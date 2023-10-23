using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.AuthModule.RequestDtos
{
    public class UserTokenDto
    {
        public int UserId { get; set; }
        public string NationalId { get; set; }
        public int RoleId { get; set; }
    }
}
