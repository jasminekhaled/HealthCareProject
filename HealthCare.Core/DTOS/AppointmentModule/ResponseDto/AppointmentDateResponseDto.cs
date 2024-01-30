using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.AppointmentModule.ResponseDto
{
    public class AppointmentDateResponseDto
    {
        public int Id { get; set; }
        public string DayName { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
    }
}
