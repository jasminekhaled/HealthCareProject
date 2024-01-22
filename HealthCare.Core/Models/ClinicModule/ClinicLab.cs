using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Core.Models.HospitalModule;

namespace HealthCare.Core.Models.ClinicModule
{
    public class ClinicLab
    {
        public int Id { get; set; }
        public int SpecializationId { get; set; }
        public Specialization Specialization { get; set; }
        public HospitalClinicLab HospitalClinicLab { get; set; }
        public List<ClinicLabDoctor> clinicLabDoctors { get; set; }


    }
}
