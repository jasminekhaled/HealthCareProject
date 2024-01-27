using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.AppointmentModule
{
    public class LabReservation : Reservation
    {
        public int LabAppointmentDateId { get; set; }
        public LabAppointmentDate LabAppointmentDate { get; set; }
        public int LabAppointmentId { get; set; }
        public LabAppointment LabAppointment { get; set; }
    }
}
