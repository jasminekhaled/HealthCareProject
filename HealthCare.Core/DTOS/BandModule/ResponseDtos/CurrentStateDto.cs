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
        public int? OxygenLevel { get; set; }
        public int? HeartRate { get; set; }
        public int? BodyTemperature { get; set; }
        public int? BloodPressure { get; set; }
    }
}
