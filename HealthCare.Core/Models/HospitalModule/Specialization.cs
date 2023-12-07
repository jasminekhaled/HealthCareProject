using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Core.Models.DoctorModule;

namespace HealthCare.Core.Models.HospitalModule
{
    public class Specialization
    {
        public int Id { get; set; }
        public string  Name { get; set; }
        public List<DoctorSpecialization> DoctorSpecialization { get; set; }
    }
}
