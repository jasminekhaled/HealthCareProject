using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.AppointmentModule
{
    public class XrayAppointmentDate : AppointmentDate
    {
        public int XrayAppointmentId { get; set; }
        public XrayAppointment XrayAppointment { get; set; }
        public List<XrayReservation> XrayReservations { get; set; }
    }
}
