using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.AppointmentModule.ResponseDto
{
    public class PatientReservationDto
    {
        public DateOnly Date { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public string HospitalName { get; set; }
        public string ClinicName { get; set; }
        public string DoctorName { get; set; }
        public string DoctorImagePath { get; set; }
        public string Price { get; set; }

    }
}
