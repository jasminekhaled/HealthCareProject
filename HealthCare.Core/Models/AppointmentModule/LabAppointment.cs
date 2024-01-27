using HealthCare.Core.Models.ClinicModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.AppointmentModule
{
    public class LabAppointment : Appointment
    {
        public int LabId { get; set; }
        public Lab Lab { get; set; }
        public List<LabAppointmentDate> LabAppointmentDates { get; set; }
        public List<LabReservation> LabReservations { get; set; }
    }
}
