using HealthCare.Core.Models.DoctorModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.ClinicModule
{
    public class SpecializationsOfLab
    {
        public int Id { get; set; }
        public int LabSpecializationId { get; set; }
        public LabSpecialization LabSpecialization { get; set; }
        public int LabId { get; set; }
        public Lab Lab { get; set; }
    }
}
