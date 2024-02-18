using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.ClinicModule.ResponseDto
{
    public class ListOfSpecificClinics
    {
        public int ClinicId { get; set; }
        public string ClinicSpecialization { get; set; }
        public string ClinicImagePath { get; set; }
        public int HospitalId { get; set; }
        public string HospitalName { get; set; }
        public string HospitalAddress { get; set; }
        public string HospitalGovernorate { get; set; }
        public string HospitalDescription { get; set; }
        public string HospitalImagePath { get; set; }

    }
}
