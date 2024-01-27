using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.AppointmentModule
{
    public class XrayReservation : Reservation
    {
        public int XrayAppointmentDateId { get; set; }
        public XrayAppointmentDate XrayAppointmentDate { get; set; }
        public int XrayAppointmentId { get; set; }
        public XrayAppointment XrayAppointment { get; set; }
    }
}
