using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.AppointmentModule
{
    public class LabAppointmentDate : AppointmentDate
    {
        public int LabAppointmentId { get; set; }
        public LabAppointment LabAppointment { get; set; }
        public List<LabReservation> LabReservations { get; set; }
    }
}
