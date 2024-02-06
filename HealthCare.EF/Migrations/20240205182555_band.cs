using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.EF.Migrations
{
    public partial class band : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_UploadedFileId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_UploadedFileId",
                table: "Doctors");

            migrationBuilder.AlterColumn<int>(
                name: "UploadedFileId",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UploadedFileId",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UploadedFileId",
                table: "Patients",
                column: "UploadedFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_UploadedFileId",
                table: "Doctors",
                column: "UploadedFileId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_UploadedFileId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_UploadedFileId",
                table: "Doctors");

            migrationBuilder.AlterColumn<int>(
                name: "UploadedFileId",
                table: "Patients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UploadedFileId",
                table: "Doctors",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
        }
    }
}
