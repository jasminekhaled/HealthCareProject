using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.EF.Migrations
{
    public partial class seedingData3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "UploadedFiles",
               columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
               values: new object[] { 301, "blood (1).png", "3o43otyi.a2j", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\SpecializationImages\\3o43otyi.a2j" }
           );

            migrationBuilder.InsertData(
               table: "Specializations",
               columns: new[] { "Id", "Name", "UploadedFileId" },
               values: new object[] { 81, "Hematology", 301 }
           );



            migrationBuilder.InsertData(
               table: "UploadedFiles",
               columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
               values: new object[] { 302, "heart (1).png", "vskyzecw.due", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\SpecializationImages\\vskyzecw.due" }
           );

            migrationBuilder.InsertData(
               table: "Specializations",
               columns: new[] { "Id", "Name", "UploadedFileId" },
               values: new object[] { 82, "Cardiology", 302 }
           );


            migrationBuilder.InsertData(
               table: "UploadedFiles",
               columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
               values: new object[] { 303, "liver.png", "13znzogm.g1v", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\SpecializationImages\\13znzogm.g1v" }
           );

            migrationBuilder.InsertData(
               table: "Specializations",
               columns: new[] { "Id", "Name", "UploadedFileId" },
               values: new object[] { 83, "Hepatology", 303 }
           );


            migrationBuilder.InsertData(
               table: "UploadedFiles",
               columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
               values: new object[] { 304, "kidney.png", "m115x4sc.hao", "image/png",
                   "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\SpecializationImages\\m115x4sc.hao" }
           );

            migrationBuilder.InsertData(
               table: "Specializations",
               columns: new[] { "Id", "Name", "UploadedFileId" },
               values: new object[] { 84, "nephrology", 304 }
           );



            migrationBuilder.InsertData(
               table: "UploadedFiles",
               columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
               values: new object[] { 305, "eye.png", "aluemst2.rov", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\SpecializationImages\\aluemst2.rov" }
           );

            migrationBuilder.InsertData(
               table: "Specializations",
               columns: new[] { "Id", "Name", "UploadedFileId" },
               values: new object[] { 85, "Ophthalmology", 305 }
           );



            migrationBuilder.InsertData(
               table: "UploadedFiles",
               columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
               values: new object[] { 306, "otorhinolaryngology.png", "tc1d51gk.w4x", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\SpecializationImages\\tc1d51gk.w4x" }
           );

            migrationBuilder.InsertData(
               table: "Specializations",
               columns: new[] { "Id", "Name", "UploadedFileId" },
               values: new object[] { 86, "Ear, nose and throat", 306 }
           );



            migrationBuilder.InsertData(
               table: "UploadedFiles",
               columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
               values: new object[] { 307, "brain.png", "f1ouy0rq.upt", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\SpecializationImages\\f1ouy0rq.upt" }
           );

            migrationBuilder.InsertData(
               table: "Specializations",
               columns: new[] { "Id", "Name", "UploadedFileId" },
               values: new object[] { 87, "Neurology", 307 }
           );



            migrationBuilder.InsertData(
               table: "UploadedFiles",
               columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
               values: new object[] { 308, "thinking.png", "ttel1eo1.jhr", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\SpecializationImages\\ttel1eo1.jhr" }
           );

            migrationBuilder.InsertData(
               table: "Specializations",
               columns: new[] { "Id", "Name", "UploadedFileId" },
               values: new object[] { 88, "Psychiatry", 308 }
           );



            migrationBuilder.InsertData(
               table: "UploadedFiles",
               columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
               values: new object[] { 309, "gallery.png", "mb0bfsxc.u51", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\SpecializationImages\\mb0bfsxc.u51" }
           );

            migrationBuilder.InsertData(
               table: "Specializations",
               columns: new[] { "Id", "Name", "UploadedFileId" },
               values: new object[] { 89, "Dermatology", 309 }
           );



            migrationBuilder.InsertData(
               table: "UploadedFiles",
               columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
               values: new object[] { 310, "baby.png", "5tyfdzpf.axp", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\SpecializationImages\\5tyfdzpf.axp" }
           );

            migrationBuilder.InsertData(
               table: "Specializations",
               columns: new[] { "Id", "Name", "UploadedFileId" },
               values: new object[] { 90, "Pediatrics", 310 }
           );



            migrationBuilder.InsertData(
               table: "UploadedFiles",
               columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
               values: new object[] { 311, "pregnant.png", "y0nrlwdz.53g", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\SpecializationImages\\y0nrlwdz.53g" }
           );

            migrationBuilder.InsertData(
               table: "Specializations",
               columns: new[] { "Id", "Name", "UploadedFileId" },
               values: new object[] { 91, "Obstetrics and Gynecology", 311 }
           );
            


            migrationBuilder.InsertData(
               table: "UploadedFiles",
               columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
               values: new object[] { 312, "scalpel.png", "ttqetw4w.zzm", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\SpecializationImages\\ttqetw4w.zzm" }
           );

            migrationBuilder.InsertData(
               table: "Specializations",
               columns: new[] { "Id", "Name", "UploadedFileId" },
               values: new object[] { 92, "General Surgery", 312 }
           );



            migrationBuilder.InsertData(
               table: "UploadedFiles",
               columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
               values: new object[] { 313, "tooth.png", "mc0k0453.5hw", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\SpecializationImages\\mc0k0453.5hw" }
           );

            migrationBuilder.InsertData(
               table: "Specializations",
               columns: new[] { "Id", "Name", "UploadedFileId" },
               values: new object[] { 93, "Dentisry", 313 }
           );



            migrationBuilder.InsertData(
               table: "UploadedFiles",
               columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
               values: new object[] { 314, "chemical-analysis (1).png", "pp55czhl.dua", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\LabSpecializationImages\\pp55czhl.dua" }
           );

            migrationBuilder.InsertData(
               table: "LabSpecializations",
               columns: new[] { "Id", "Name", "UploadedFileId", "Description" },
               values: new object[] { 44, "Cancer screening Tests", 314, "description of Cancer screening Tests" }
           );



            migrationBuilder.InsertData(
               table: "UploadedFiles",
               columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
               values: new object[] { 315, "chemical-analysis (1).png", "pp55czhl.dua", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\LabSpecializationImages\\pp55czhl.dua" }
           );

            migrationBuilder.InsertData(
               table: "LabSpecializations",
               columns: new[] { "Id", "Name", "UploadedFileId", "Description" },
               values: new object[] { 45, "Blood clotting tests", 315, "description of Blood clotting tests" }
           );



            migrationBuilder.InsertData(
              table: "UploadedFiles",
              columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
              values: new object[] { 316, "chemical-analysis (1).png", "pp55czhl.dua", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\LabSpecializationImages\\pp55czhl.dua" }
          );

            migrationBuilder.InsertData(
               table: "LabSpecializations",
               columns: new[] { "Id", "Name", "UploadedFileId", "Description" },
               values: new object[] { 46, "Hormone tests", 316, "description of Hormone tests" }
           );



            migrationBuilder.InsertData(
              table: "UploadedFiles",
              columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
              values: new object[] { 317, "chemical-analysis (1).png", "pp55czhl.dua", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\LabSpecializationImages\\pp55czhl.dua" }
          );

            migrationBuilder.InsertData(
               table: "LabSpecializations",
               columns: new[] { "Id", "Name", "UploadedFileId", "Description" },
               values: new object[] { 47, "Immune system tests", 317, "description of Immune system tests" }
           );



            migrationBuilder.InsertData(
              table: "UploadedFiles",
              columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
              values: new object[] { 318, "chemical-analysis (1).png", "pp55czhl.dua", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\LabSpecializationImages\\pp55czhl.dua" }
          );

            migrationBuilder.InsertData(
               table: "LabSpecializations",
               columns: new[] { "Id", "Name", "UploadedFileId", "Description" },
               values: new object[] { 48, "Genetic tests", 318, "description of Genetic tests" }
           );



            migrationBuilder.InsertData(
              table: "UploadedFiles",
              columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
              values: new object[] { 319, "chemical-analysis (1).png", "pp55czhl.dua", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\LabSpecializationImages\\pp55czhl.dua" }
          );

            migrationBuilder.InsertData(
               table: "LabSpecializations",
               columns: new[] { "Id", "Name", "UploadedFileId", "Description" },
               values: new object[] { 49, "Allergy tests", 319, "description of Allergy tests" }
           );



            migrationBuilder.InsertData(
              table: "UploadedFiles",
              columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
              values: new object[] { 320, "chemical-analysis (1).png", "pp55czhl.dua", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\LabSpecializationImages\\pp55czhl.dua" }
          );

            migrationBuilder.InsertData(
               table: "LabSpecializations",
               columns: new[] { "Id", "Name", "UploadedFileId", "Description" },
               values: new object[] { 50, "Blood disease tests", 320, "description of Blood disease tests" }
           );



            migrationBuilder.InsertData(
              table: "UploadedFiles",
              columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
              values: new object[] { 321, "chemical-analysis (1).png", "pp55czhl.dua", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\LabSpecializationImages\\pp55czhl.dua" }
          );

            migrationBuilder.InsertData(
               table: "LabSpecializations",
               columns: new[] { "Id", "Name", "UploadedFileId", "Description" },
               values: new object[] { 51, "Renal function tests", 321, "description of Renal function tests" }
           );



            migrationBuilder.InsertData(
              table: "UploadedFiles",
              columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
              values: new object[] { 322, "mri.png", "yidxhjhs.jmo", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\XraySpecializationImages\\yidxhjhs.jmo" }
          );

            migrationBuilder.InsertData(
               table: "XraySpecializations",
               columns: new[] { "Id", "Name", "UploadedFileId" },
               values: new object[] { 32, "Magnetic Resonance Imaging", 322 }
           );



            migrationBuilder.InsertData(
              table: "UploadedFiles",
              columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
              values: new object[] { 323, "rays.png", "jshiuuzx.ddq", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\XraySpecializationImages\\jshiuuzx.ddq" }
          );

            migrationBuilder.InsertData(
               table: "XraySpecializations",
               columns: new[] { "Id", "Name", "UploadedFileId" },
               values: new object[] { 33, "Ultrasound Imaging", 323 }
           );



            migrationBuilder.InsertData(
              table: "UploadedFiles",
              columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
              values: new object[] { 324, "medical.png", "a4sc2uxy.3q2", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\XraySpecializationImages\\a4sc2uxy.3q2" }
          );

            migrationBuilder.InsertData(
               table: "XraySpecializations",
               columns: new[] { "Id", "Name", "UploadedFileId" },
               values: new object[] { 34, "X-ray", 324 }
           );



            migrationBuilder.InsertData(
              table: "UploadedFiles",
              columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
              values: new object[] { 325, "cancer.png", "5ukiorxy.vt1", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\XraySpecializationImages\\5ukiorxy.vt1" }
          );

            migrationBuilder.InsertData(
               table: "XraySpecializations",
               columns: new[] { "Id", "Name", "UploadedFileId" },
               values: new object[] { 35, "Fluoroscopy", 325 }
           );





            migrationBuilder.InsertData(
              table: "UploadedFiles",
              columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
              values: new object[] { 326, "986-9863814_image-is-not-available-girl.png", "5oeediqd.nsk", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\HospitalAdminImages\\5oeediqd.nsk" }
          );

            migrationBuilder.InsertData(
               table: "Users",
               columns: new[] { "Id", "NationalId", "Email", "UserName", "PassWord", "UploadedFileId", "VerificationCode", "Role" },
               values: new object[] { 1200, "00000000000000", "SuperAdminAdmin1@gmail.com", "SuperAdminAdmin1", "ae1af191aa9df3dadab19ef84bb5dfc1971a134f220f5c065f138600fd2f9760", 326, null, "SuperAdmin" }
           );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Specializations]");
            migrationBuilder.Sql("DELETE FROM [UploadedFiles]");
            migrationBuilder.Sql("DELETE FROM [LabSpecializations]");
            migrationBuilder.Sql("DELETE FROM [XraySpecializations]");
            migrationBuilder.Sql("DELETE FROM [Users]");

        }
    }
}
