using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.PatientModule.ResponseDto
{
    public class FileResponseDto
    {
        public int FileId { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string FileDescription { get; set; }
        
    }
}
