using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.HospitalModule
{
    public class ClinicLabDoctor
    {
        public int Id { get; set; }
        public int ClinicLabId { get; set; }
        public ClinicLab ClinicLab { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
