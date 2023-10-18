using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.HospitalModule
{
    public class ClinicLab
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int HospitalId { get; set; }
        public Hospital Hospital { get; set; }
        public List<ClinicLabDoctor> clinicLabDoctors { get; set; }


    }
}
