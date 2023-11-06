using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.HospitalModule.RequestDto
{
    public class HospitalRequestDto
    {
        [MinLength(length: 5)]
        [MaxLength(length: 150)]
        public string Name { get; set; }
        [MinLength(length: 5)]
        [MaxLength(length: 150)]
        public string Address { get; set; }
        [MinLength(length: 5)]
        [MaxLength(length: 500)]
        public string Description { get; set; }
        public int GovernorateId { get; set; }
        public IFormFile Image { get; set; }
    }
}
