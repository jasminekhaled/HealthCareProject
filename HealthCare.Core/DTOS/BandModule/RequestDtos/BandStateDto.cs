using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.BandModule.RequestDtos
{
    public class BandStateDto
    {
        public string Oxygen { get; set; }
        public string HeartRate { get; set; }
        public string Temperature { get; set; }
        public string BloodPressure { get; set; }
        public string BloodSugar { get; set; }
    }
}
