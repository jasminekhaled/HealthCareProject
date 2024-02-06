using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.DoctorModule.RequestDtos
{
    public class DoctorRequestDto
    {
        public string NationalId { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(length: 10)]
        [MaxLength(length: 100)]
        public string UserName { get; set; }
        [MinLength(length: 2)]
        [MaxLength(length: 20)]
        public string FirstName { get; set; }
        [MinLength(length: 2)]
        [MaxLength(length: 20)]
        public string LastName { get; set; }
        [MinLength(length: 15)]
        [MaxLength(length: 100)]
        public string PassWord { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public List<int> SpecializationId { get; set; }
        public List<int> HospitalId { get; set; }
    }
}
