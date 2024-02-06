using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.DTOS.BandModule.RequestDtos
{
    public class ChangeBandPatientDto
    {
        public string PatientNationalId { get; set; }
        public int? RoomNum { get; set; }
    }
}
