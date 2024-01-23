using HealthCare.Core.Models.HospitalModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.ClinicModule
{
    public class Xray
    {
        public int Id { get; set; }
        public int XraySpecializationId { get; set; }
        public XraySpecialization XraySpecialization { get; set; }
        public int HospitalId { get; set; }
        public Hospital Hospital { get; set; }
        public List<XrayDoctor> XrayDoctors { get; set; }
    }
}
