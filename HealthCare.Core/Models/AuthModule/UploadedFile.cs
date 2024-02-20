using HealthCare.Core.Models.ClinicModule;
using HealthCare.Core.Models.DoctorModule;
using HealthCare.Core.Models.HospitalModule;
using HealthCare.Core.Models.PatientModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.AuthModule
{
    public class UploadedFile
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? StoredFileName { get; set; }
        public string? ContentType { get; set; }
        public string? FilePath { get; set; }
        public Hospital Hospital { get; set; }
        public HospitalAdmin HospitalAdmin { get; set; }
        public Patient? Patient { get; set; }
        public Doctor Doctor { get; set; }
        public User User { get; set; }
        public Specialization Specialization { get; set; }
        public XraySpecialization XraySpecialization { get; set; }
        public LabSpecialization LabSpecialization { get; set; }


        public static string DefaultImagePath = "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\DefaultImage";
    }
}
