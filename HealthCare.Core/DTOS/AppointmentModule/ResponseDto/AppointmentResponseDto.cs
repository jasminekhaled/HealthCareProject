using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.AppointmentModule.ResponseDto
{
    public class AppointmentResponseDto
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public string DoctorName { get; set; }
        public string DoctorImagePath { get; set; }
        public float DoctorRate { get; set; }
        public string DoctorDiscription { get; set; }
        public AppointmentDateResponseDto AppointmentDates { get; set; }

    }
}
