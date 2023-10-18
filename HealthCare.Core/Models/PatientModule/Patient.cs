using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.PatientModule
{
    public class Patient 
    {
        public int Id { get; set; }
        public int NationalId { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(length: 10)]
        [MaxLength(length: 100)]
        public string FullName { get; set; }

        [MinLength(length: 15)]
        [MaxLength(length: 100)]
        public string PassWord { get; set; }
        public bool IsEmailConfirmed { get; set; }
        [StringLength(11)]
        public string PhoneNumber { get; set; }
        public string? VerificationCode { get; set; }

    }
}
