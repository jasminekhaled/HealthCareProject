using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.PatientModule.RequestDto
{
    public class EditMedicalHistoryDto
    {
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? FriendName { get; set; }
        public string? FriendPhoneNum { get; set; }
        public string? FriendAddress { get; set; }
        public bool? AnyDiseases { get; set; }
        public string? DiseasesDescribtion { get; set; }
        public bool? AnySurgery { get; set; }
        public string? SurgeryDescribtion { get; set; }
        public bool? AnyAllergy { get; set; }
        public string? AllergyDescribtion { get; set; }
        public bool? AnyMedicine { get; set; }
        public string? MedicineDescribtion { get; set; }
        public bool? MedicalInsurance { get; set; }
        public string? MedicalInsuranceDescribtion { get; set; }
    }
}
