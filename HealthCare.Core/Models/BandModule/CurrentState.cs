using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.BandModule
{
    public class CurrentState
    {
        public int Id { get; set; }
        public int OxygenLevel { get; set; }
        public int HeartBeat { get; set; }
        public int BodyTemperature { get; set; }
    }
}
