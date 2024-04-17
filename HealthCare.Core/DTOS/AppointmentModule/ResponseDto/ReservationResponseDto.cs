using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.AppointmentModule.ResponseDto
{
    public class ReservationResponseDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string PatientName { get; set; }
        public string PatientPhoneNum { get; set; }
        public int MedicalHistoryId { get; set; }
    }
}
