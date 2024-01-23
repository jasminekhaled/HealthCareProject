using HealthCare.Core.Models.AuthModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.ClinicModule
{
    public class XraySpecialization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UploadedFileId { get; set; }
        public UploadedFile UploadedFile { get; set; }
        public List<Xray> Xrays { get; set; }
    }
}
