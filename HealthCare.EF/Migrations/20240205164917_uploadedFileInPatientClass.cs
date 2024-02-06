using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.EF.Migrations
{
    public partial class uploadedFileInPatientClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UploadedFileId",
                table: "Patients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UploadedFileId",
                table: "Doctors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UploadedFileId",
                table: "Patients",
                column: "UploadedFileId",
                unique: true,
                filter: "[UploadedFileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_UploadedFileId",
                table: "Doctors",
                column: "UploadedFileId",
                unique: true,
                filter: "[UploadedFileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_UploadedFiles_UploadedFileId",
                table: "Doctors",
                column: "UploadedFileId",
                principalTable: "UploadedFiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_UploadedFiles_UploadedFileId",
                table: "Patients",
                column: "UploadedFileId",
                principalTable: "UploadedFiles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_UploadedFiles_UploadedFileId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_UploadedFiles_UploadedFileId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_UploadedFileId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_UploadedFileId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "UploadedFileId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "UploadedFileId",
                table: "Doctors");
        }
    }
}
