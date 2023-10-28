using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.HospitalModule
{
    public class Doctor 
    {
        public int Id { get; set; }
        public string NationalId { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(length: 10)]
        [MaxLength(length: 100)]
        public string UserName { get; set; }

        [MinLength(length: 15)]
        [MaxLength(length: 100)]
        public string PassWord { get; set; }
        public string ImagePath { get; set; }
        public int Rate { get; set; }
        public List<HospitalDoctor> hospitalDoctors { get; set; }
        public List<ClinicLabDoctor> clinicLabDoctors { get; set; }


    }
}
