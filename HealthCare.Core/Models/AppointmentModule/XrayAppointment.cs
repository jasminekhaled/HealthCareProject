using HealthCare.Core.Models.ClinicModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.AppointmentModule
{
    public class XrayAppointment : Appointment
    {
        public int XrayId { get; set; }
        public Xray Xray { get; set; }
        public List<XrayAppointmentDate> XrayAppointmentDates { get; set; }
        public List<XrayReservation> XrayReservations { get; set; }
    }
}
