using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.AuthModule.RequestDtos
{
    public class ChangeForgettedPasswordDto
    {
        public string VerificationCode { get; set; }

        [MinLength(length: 15)]
        [MaxLength(length: 100)]
        public string NewPassWord { get; set; }
        public string ConfirmPassWord { get; set; }
    }
}
