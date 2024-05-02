using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.BandModule.ResponseDtos
{
    public class CurrentStateDto
    {
        public int Id { get; set; }
        public int? Oxygen { get; set; }
        public int? HeartRate { get; set; }
        public int? Temperature { get; set; }
        public int? BloodPressure { get; set; }
        public int? BloodSugar { get; set; }
    }
}
