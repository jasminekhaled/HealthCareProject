using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthCare.Core.Models.AuthModule;

namespace HealthCare.Core.Models.HospitalModule
{
    [Table("HospitalAdmins")]
    public class HospitalAdmin : GeneralUser
    {
        public int UploadedFileId { get; set; }
        public UploadedFile UploadedFile { get; set; }
        public AdminOfHospital AdminOfHospital { get; set; }

    }
}
