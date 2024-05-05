using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.EF.Migrations
{
    public partial class seedingData2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "UploadedFiles",
               columns: new[] { "Id", "FileName", "StoredFileName", "ContentType", "FilePath" },
               values: new object[] { 300, "newborn.png", "vlrexppj.xkm", "image/png", "G:\\WEB DEVELOPMENT\\HealthCareProject\\HealthCareAPIs\\HealthCare\\Uploads\\SpecializationImages\\vlrexppj.xkm" }
           );

            migrationBuilder.InsertData(
               table: "Specializations",
               columns: new[] { "Id", "Name", "UploadedFileId" },
               values: new object[] { 80, "Pediatric surgery", 300 }
           );

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Specializations]");
            migrationBuilder.Sql("DELETE FROM [UploadedFiles]");
        }
    }
}
