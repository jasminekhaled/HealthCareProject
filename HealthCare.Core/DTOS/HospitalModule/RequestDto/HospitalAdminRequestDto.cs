using HealthCare.Core.Models.HospitalModule;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.HospitalModule.RequestDto
{
    public class HospitalAdminRequestDto
    {

        [MinLength(length: 10)]
        [MaxLength(length: 100)]
        public string UserName { get; set; }
        public string NationalId { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(length: 15)]
        [MaxLength(length: 100)]
        public string PassWord { get; set; }
        public IFormFile Image { get; set; }
    }
}
