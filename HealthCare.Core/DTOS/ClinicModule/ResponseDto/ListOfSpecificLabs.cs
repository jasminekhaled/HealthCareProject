using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.ClinicModule.ResponseDto
{
    public class ListOfSpecificLabs
    {
        public int LabId { get; set; }
        public string LabName { get; set; }
        public List<string> LabSpecializationNames { get; set; }
        public string LabImagePath { get; set; }
        public int HospitalId { get; set; }
        public string HospitalName { get; set; }
        public string HospitalAddress { get; set; }
        public string HospitalGovernorate { get; set; }
        public string HospitalDescription { get; set; }
        public string HospitalImagePath { get; set; }
    }
}
