using HealthCare.Core.Models.AuthModule;
using HealthCare.Core.Models.HospitalModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.DoctorModule
{
    [Table("Doctors")]
    public class Doctor : GeneralUser
    {
        public string FullName { get; set; }
        public string Description { get; set; }
        public int Rate { get; set; }
        public List<DoctorSpecialization> DoctorSpecialization { get; set; }
        public List<HospitalDoctor> hospitalDoctors { get; set; }
        public List<ClinicLabDoctor> clinicLabDoctors { get; set; }
        public List<RateDoctor> RateDoctor { get; set; }


    }
}
