using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.DoctorModule.RequestDtos
{
    public class EditDoctorDto
    {

        [MinLength(length: 5)]
        [MaxLength(length: 40)]
        public string? FullName { get; set; }
        [MinLength(length: 15)]
        [MaxLength(length: 100)]
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}
