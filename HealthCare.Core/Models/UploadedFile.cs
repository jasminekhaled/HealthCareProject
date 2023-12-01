using HealthCare.Core.Models.HospitalModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models
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
        public Doctor Doctor { get; set; }
    }
}
