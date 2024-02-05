using HealthCare.Core.Models.DoctorModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.BandModule
{
    public class SavedBand
    {
        public int Id { get; set; }
        public int BandId { get; set; }
        public Band Band { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

    }   
}
