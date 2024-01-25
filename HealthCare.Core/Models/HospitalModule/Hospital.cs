using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Core.Models.AuthModule;
using HealthCare.Core.Models.ClinicModule;
using HealthCare.Core.Models.DoctorModule;

namespace HealthCare.Core.Models.HospitalModule
{
    public class Hospital
    {
        public int Id { get; set; }
        [MinLength(length: 5)]
        [MaxLength(length: 150)]
        public string Name { get; set; }
        [MinLength(length: 5)]
        [MaxLength(length: 150)]
        public string Address { get; set; }

        [MinLength(length: 5)]
        [MaxLength(length: 500)]
        public string Description { get; set; }
        public int UploadedFileId { get; set; }
        public UploadedFile UploadedFile { get; set; }
        public List<Xray> Xrays { get; set; }
        public List<ClinicLab> ClinicLabs { get; set; }
        public List<Lab> Labs { get; set; }
        public List<HospitalDoctor> hospitalDoctors { get; set; }
        public List<AdminOfHospital> AdminOfHospitals { get; set; }
        public HospitalGovernorate HospitalGovernorate { get; set; }
        
    }
}
