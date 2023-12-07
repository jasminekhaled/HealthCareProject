using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Core.Models.HospitalModule;

namespace HealthCare.Core.Models.DoctorModule
{
    public class DoctorSpecialization
    {
        public int Id { get; set; }
        public int SpecializationId { get; set; }
        public Specialization Specialization { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
