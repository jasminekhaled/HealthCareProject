using HealthCare.Core.Models.DoctorModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.ClinicModule
{
    public class XrayDoctor
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public int XrayId { get; set; }
        public Xray Xray { get; set; }
    }

}
