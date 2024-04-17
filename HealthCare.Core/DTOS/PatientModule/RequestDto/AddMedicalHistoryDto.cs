using HealthCare.Core.Models.PatientModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.PatientModule.RequestDto
{
    public class AddMedicalHistoryDto
    {
        public string Address { get; set; }
        [Required]
        public bool Gender { get; set; }
        [Required]
        public string BirthDate { get; set; }
        public string BloodType { get; set; }
        public string FriendName { get; set; }
        public string FriendPhoneNum { get; set; }
        public string FriendAddress { get; set; }
        [Required]
        public bool AnyDiseases { get; set; }
        public string? DiseasesDescribtion { get; set; }
        [Required]
        public bool AnySurgery { get; set; }
        public string? SurgeryDescribtion { get; set; }
        [Required]
        public bool AnyAllergy { get; set; }
        public string? AllergyDescribtion { get; set; }
        [Required]
        public bool AnyMedicine { get; set; }
        public string? MedicineDescribtion { get; set; }
        [Required]
        public bool MedicalInsurance { get; set; }
        public string? MedicalInsuranceDescribtion { get; set; }
        [Required]
        public bool Endorsement { get; set; }
    }
}
