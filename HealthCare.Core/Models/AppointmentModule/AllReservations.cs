using HealthCare.Core.Models.DoctorModule;
using HealthCare.Core.Models.PatientModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.AppointmentModule
{
    public class AllReservations
    {
        public int Id { get; set; } 
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public int RoomId { get; set; }
        public string Type { get; set; }
        public string price { get; set; }
        public string Date { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int RoomAppointmentDateId { get; set; }
        public int RoomReservationId { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
