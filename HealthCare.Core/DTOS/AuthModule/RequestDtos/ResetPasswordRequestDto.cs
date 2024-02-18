using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.AuthModule.RequestDtos
{
    public class ResetPasswordRequestDto
    {

        public string PassWord { get; set; }

        [MinLength(length: 15)]
        [MaxLength(length: 100)]
        public string NewPassWord { get; set; }
        public string ConfirmNewPassWord { get; set; }
    }
}
