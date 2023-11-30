using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.EF.Migrations
{
    public partial class hospitalAdminupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "HospitalAdmins");

            migrationBuilder.AddColumn<int>(
                name: "UploadedFileId",
                table: "HospitalAdmins",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HospitalAdmins_UploadedFileId",
                table: "HospitalAdmins",
                column: "UploadedFileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HospitalAdmins_UploadedFiles_UploadedFileId",
                table: "HospitalAdmins",
                column: "UploadedFileId",
                principalTable: "UploadedFiles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HospitalAdmins_UploadedFiles_UploadedFileId",
                table: "HospitalAdmins");

            migrationBuilder.DropIndex(
                name: "IX_HospitalAdmins_UploadedFileId",
                table: "HospitalAdmins");

            migrationBuilder.DropColumn(
                name: "UploadedFileId",
                table: "HospitalAdmins");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "HospitalAdmins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
