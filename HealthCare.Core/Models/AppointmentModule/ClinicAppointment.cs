using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Core.Models.ClinicModule;
using HealthCare.Core.Models.DoctorModule;

namespace HealthCare.Core.Models.AppointmentModule
{
    public class ClinicAppointment : Appointment
    {
        public int ClinicLabId { get; set; }
        public ClinicLab ClinicLab { get; set; }
        public List<ClinicAppointmentDate> ClinicAppointmentDates { get; set; }
        public List<ClinicReservation> ClinicReservations { get; set; }


    }
}
