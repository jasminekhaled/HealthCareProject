using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.HospitalModule.ResponseDto
{
    public class HospitalDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int GovernorateId { get; set; }
        public string Governorate { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}
