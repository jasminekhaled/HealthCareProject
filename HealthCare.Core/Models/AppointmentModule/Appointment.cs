using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Core.Models.HospitalModule;

namespace HealthCare.Core.Models.AppointmentModule
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public int Price { get; set; }
        public int ClinicLabId { get; set; }
        public ClinicLab ClinicLab { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

    }
}
