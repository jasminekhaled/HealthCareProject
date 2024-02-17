using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.PatientModule
{
    public class MedicalHistoryFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string StoredFileName { get; set; }
        public string ContentType { get; set; }
        public string FilePath { get; set; }
        public string Type { get; set; }
        public string? Description { get; set; }
        public string AddedBy { get; set; }
        public int MedicalHistoryId { get; set; }
        public MedicalHistory MedicalHistory { get; set; }


        public static string Xray = "Xray";
        public static string Test = "Test";
    }
}
