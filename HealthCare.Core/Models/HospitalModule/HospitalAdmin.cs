using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.HospitalModule
{
    public class HospitalAdmin
    {
        public int Id { get; set; }
        
        [MinLength(length: 10)]
        [MaxLength(length: 100)]
        public string UserName { get; set; }
        public string NationalId { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(length: 15)]
        [MaxLength(length: 100)]
        public string PassWord { get; set; }
        public string ImagePath { get; set; }
        public AdminOfHospital AdminOfHospital { get; set; }

    }
}
