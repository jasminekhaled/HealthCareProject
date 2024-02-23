using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.DoctorModule.ResponseDtos
{
    public class EditDoctorResponseDto
    {
        public int Id { get; set; }
        public string NationalId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public List<SpecializationDto> specializations { get; set; }
        
    }
}
