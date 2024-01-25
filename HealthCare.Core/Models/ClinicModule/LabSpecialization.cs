using HealthCare.Core.Models.AuthModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.ClinicModule
{
    public class LabSpecialization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UploadedFileId { get; set; }
        public UploadedFile UploadedFile { get; set; }
        public List<SpecializationsOfLab> SpecializationsOfLab { get; set; }
    }
}
