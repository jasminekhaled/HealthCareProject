using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Models.AuthModule
{
    [Table("Users")]
    public class User : GeneralUser
    {
        public string? VerificationCode { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public int RoleId { get; set; }
        public UserRole UserRole { get; set; }
        public int UploadedFileId { get; set; }
        public UploadedFile UploadedFile { get; set; }

    }
}
