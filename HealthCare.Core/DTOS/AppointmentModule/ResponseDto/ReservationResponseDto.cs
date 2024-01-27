using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.AppointmentModule.ResponseDto
{
    public class ReservationResponseDto
    {
        public DateOnly Date { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public string PatientName { get; set; }
        public string PatientPhoneNum { get; set; }
    }
}
