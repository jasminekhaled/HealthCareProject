using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Core.Models.AppointmentModule;
using HealthCare.Core.Models.HospitalModule;

namespace HealthCare.Core.Models.ClinicModule
{
    public class ClinicLab
    {
        public int Id { get; set; }
        public int SpecializationId { get; set; }
        public Specialization Specialization { get; set; }
        public int HospitalId { get; set; }
        public Hospital Hospital { get; set; }
        public List<ClinicLabDoctor> clinicLabDoctors { get; set; }
        public List<ClinicAppointment> ClinicAppointments { get; set; }



    }
}
