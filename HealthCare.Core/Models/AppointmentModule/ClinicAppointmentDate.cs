using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.AppointmentModule
{
    public class ClinicAppointmentDate : AppointmentDate
    {
        public int ClinicAppointmentId { get; set; }
        public ClinicAppointment ClinicAppointment { get; set; }
        public List<ClinicReservation> ClinicReservations { get; set; }
    }
}
