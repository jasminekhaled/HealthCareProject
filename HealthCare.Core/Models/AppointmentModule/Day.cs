using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.AppointmentModule
{
    public class Day
    {
        public int Id { get; set; }
        public String DayName { get; set; }
        public List<ClinicAppointmentDate> ClinicAppointmentDates { get; set; }
        public List<XrayAppointmentDate> XrayAppointmentDates { get; set; }
        public List<LabAppointmentDate> LabAppointmentDates { get; set; }
    }
}
