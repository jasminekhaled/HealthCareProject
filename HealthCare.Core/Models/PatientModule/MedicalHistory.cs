using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.PatientModule
{
    public class MedicalHistory
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public bool Gender { get; set; }
        public string BirthDate { get; set; }
        public string FriendName { get; set; }
        public string FriendPhoneNum { get; set; }
        public string FriendAddress { get; set; }
        public bool AnyDiseases { get; set; }
        public string? DiseasesDescribtion { get; set; }
        public bool AnySurgery { get; set; }
        public string? SurgeryDescribtion { get; set; }
        public bool AnyAllergy { get; set; }
        public string? AllergyDescribtion { get; set; }
        public bool AnyMedicine { get; set; }
        public string? MedicineDescribtion { get; set; }
        public bool MedicalInsurance { get; set; }
        public string? MedicalInsuranceDescribtion { get; set; }
        public bool Endorsement  { get; set; }
        public Patient Patient { get; set; }
        public List<MedicalHistoryFile> MedicalHistoryFiles { get; set; }
    }
}
