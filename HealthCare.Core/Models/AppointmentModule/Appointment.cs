using HealthCare.Core.Models.DoctorModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.AppointmentModule
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Price { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
