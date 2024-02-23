using HealthCare.Core.DTOS.HospitalModule.ResponseDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.DoctorModule.ResponseDtos
{
    public class DoctorDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string NationalId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; } 
        public string Description { get; set; }
        public string Rate { get; set; }
        public string ImagePath { get; set; }
        public List<SpecializationDto> Specializations { get; set; }
        public List<ListOfHospitalDto> Hospitals { get; set; }
    }
}
