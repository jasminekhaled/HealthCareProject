using HealthCare.Core.Models.HospitalModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.ClinicModule
{
    public class Lab
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SpecializationsOfLab> SpecializationsOfLab { get; set; }
        public int HospitalId { get; set; }
        public Hospital Hospital { get; set; }
        public List<LabDoctor> LabDoctors { get; set; }
    }
}
