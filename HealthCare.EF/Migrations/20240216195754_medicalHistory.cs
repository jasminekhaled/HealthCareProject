using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.EF.Migrations
{
    public partial class medicalHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalHistories_Patients_PatientId",
                table: "MedicalHistories");

            migrationBuilder.DropIndex(
                name: "IX_MedicalHistories_PatientId",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "MedicalHistories");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "MedicalHistories",
                newName: "FriendPhoneNum");

            migrationBuilder.AddColumn<int>(
                name: "MedicalHistoryId",
                table: "Patients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "MedicalHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AllergyDescribtion",
                table: "MedicalHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AnyAllergy",
                table: "MedicalHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AnyDiseases",
                table: "MedicalHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AnyMedicine",
                table: "MedicalHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AnySurgery",
                table: "MedicalHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "DiseasesDescribtion",
                table: "MedicalHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Endorsement",
                table: "MedicalHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FriendAddress",
                table: "MedicalHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FriendName",
                table: "MedicalHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Gender",
                table: "MedicalHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "MedicalInsurance",
                table: "MedicalHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MedicalInsuranceDescribtion",
                table: "MedicalHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicineDescribtion",
                table: "MedicalHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SurgeryDescribtion",
                table: "MedicalHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MedicalHistoryFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoredFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalHistoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalHistoryFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalHistoryFiles_MedicalHistories_MedicalHistoryId",
                        column: x => x.MedicalHistoryId,
                        principalTable: "MedicalHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_MedicalHistoryId",
                table: "Patients",
                column: "MedicalHistoryId",
                unique: true,
                filter: "[MedicalHistoryId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistoryFiles_MedicalHistoryId",
                table: "MedicalHistoryFiles",
                column: "MedicalHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_MedicalHistories_MedicalHistoryId",
                table: "Patients",
                column: "MedicalHistoryId",
                principalTable: "MedicalHistories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_MedicalHistories_MedicalHistoryId",
                table: "Patients");

            migrationBuilder.DropTable(
                name: "MedicalHistoryFiles");

            migrationBuilder.DropIndex(
                name: "IX_Patients_MedicalHistoryId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MedicalHistoryId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "AllergyDescribtion",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "AnyAllergy",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "AnyDiseases",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "AnyMedicine",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "AnySurgery",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "DiseasesDescribtion",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "Endorsement",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "FriendAddress",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "FriendName",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "MedicalInsurance",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "MedicalInsuranceDescribtion",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "MedicineDescribtion",
                table: "MedicalHistories");

            migrationBuilder.DropColumn(
                name: "SurgeryDescribtion",
                table: "MedicalHistories");

            migrationBuilder.RenameColumn(
                name: "FriendPhoneNum",
                table: "MedicalHistories",
                newName: "FullName");

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "MedicalHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalHistories_PatientId",
                table: "MedicalHistories",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalHistories_Patients_PatientId",
                table: "MedicalHistories",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
