using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.AuthModule.ResponseDtos
{
    public class SignUpResponse
    {
        public int PatientId { get; set; }
        public string NationalId { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }

        [MinLength(length: 10)]
        [MaxLength(length: 100)]
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string ImagePath { get; set; }
    }
}
