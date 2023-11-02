using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.HospitalModule
{
    public class Hospital
    {
        public int Id { get; set; }
        [MinLength(length: 5)]
        [MaxLength(length: 150)]
        public string Name { get; set; }
        [MinLength(length: 5)]
        [MaxLength(length: 150)]
        public string Address { get; set; }
        public string Government { get; set; }
        [MinLength(length: 5)]
        [MaxLength(length: 500)]
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public List<HospitalDoctor> hospitalDoctors { get; set; }
        public List<AdminOfHospital> AdminOfHospitals { get; set; }


    }
}
