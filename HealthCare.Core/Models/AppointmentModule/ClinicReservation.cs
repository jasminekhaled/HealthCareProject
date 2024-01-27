using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.AppointmentModule
{
    public class ClinicReservation : Reservation
    {
        public int ClinicAppointmentDateId { get; set; }
        public ClinicAppointmentDate ClinicAppointmentDate { get; set; }
        public int ClinicAppointmentId { get; set; }
        public ClinicAppointment ClinicAppointment { get; set; }
    }
}
