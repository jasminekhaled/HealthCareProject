using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.BandModule.ResponseDtos
{
    public class AddPublicBandDto
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public string PatientName { get; set; }
        public string PatientNationalId { get; set; }
        public string PatientImagePath { get; set; }
    }
}
