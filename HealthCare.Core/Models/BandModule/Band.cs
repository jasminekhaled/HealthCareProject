using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Core.Models.HospitalModule;
using HealthCare.Core.Models.PatientModule;

namespace HealthCare.Core.Models.BandModule
{
    public class Band
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public string Type { get; set; }
        public int? RoomNum { get; set; }
        public bool IsActive { get; set; }
        public bool Flag { get; set; }
        public int? PatientId { get; set; }
        public Patient Patient { get; set; }
        public int HospitalId { get; set; }
        public Hospital Hospital { get; set; }
        public int CurrentStateId { get; set; }
        public CurrentState CurrentState { get; set; }
        public List<SavedBand> SavedBands { get; set; }


        public static string Private = "Private";
        public static string Public = "Public";
    }
}
