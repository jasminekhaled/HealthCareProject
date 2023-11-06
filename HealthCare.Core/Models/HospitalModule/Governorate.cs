using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.HospitalModule
{
    public class Governorate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<HospitalGovernorate> HospitalGovernorates { get; set; }
    }
}
