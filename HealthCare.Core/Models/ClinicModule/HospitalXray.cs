using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Core.Models.HospitalModule;

namespace HealthCare.Core.Models.ClinicModule
{
    public class HospitalXray
    {
        public int Id { get; set; }
        public int HospitalId { get; set; }
        public Hospital Hospital { get; set; }
        public int XrayId { get; set; }
        public Xray Xray { get; set; }
    }
}
