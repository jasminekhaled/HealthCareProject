using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.EF.Migrations
{
    public partial class uploadedfiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClinicLabs_Hospitals_HospitalId",
                table: "ClinicLabs");

            migrationBuilder.DropIndex(
                name: "IX_ClinicLabs_HospitalId",
                table: "ClinicLabs");

            migrationBuilder.DropColumn(
                name: "Government",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "HospitalId",
                table: "ClinicLabs");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Hospitals",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Hospitals",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Hospitals",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UploadedFileId",
                table: "Hospitals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Governorates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Governorates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HospitalAdmins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PassWord = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HospitalAdmins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HospitalClinicLabs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HospitalId = table.Column<int>(type: "int", nullable: false),
                    ClinicLabId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HospitalClinicLabs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HospitalClinicLabs_ClinicLabs_ClinicLabId",
                        column: x => x.ClinicLabId,
                        principalTable: "ClinicLabs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HospitalClinicLabs_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UploadedFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoredFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadedFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HospitalGovernorates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HospitalId = table.Column<int>(type: "int", nullable: false),
                    GovernorateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HospitalGovernorates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HospitalGovernorates_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HospitalGovernorates_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminOfHospitals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HospitalId = table.Column<int>(type: "int", nullable: false),
                    HospitalAdminId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminOfHospitals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminOfHospitals_HospitalAdmins_HospitalAdminId",
                        column: x => x.HospitalAdminId,
                        principalTable: "HospitalAdmins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdminOfHospitals_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hospitals_UploadedFileId",
                table: "Hospitals",
                column: "UploadedFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdminOfHospitals_HospitalAdminId",
                table: "AdminOfHospitals",
                column: "HospitalAdminId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdminOfHospitals_HospitalId",
                table: "AdminOfHospitals",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_HospitalClinicLabs_ClinicLabId",
                table: "HospitalClinicLabs",
                column: "ClinicLabId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HospitalClinicLabs_HospitalId",
                table: "HospitalClinicLabs",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_HospitalGovernorates_GovernorateId",
                table: "HospitalGovernorates",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_HospitalGovernorates_HospitalId",
                table: "HospitalGovernorates",
                column: "HospitalId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Hospitals_UploadedFiles_UploadedFileId",
                table: "Hospitals",
                column: "UploadedFileId",
                principalTable: "UploadedFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hospitals_UploadedFiles_UploadedFileId",
                table: "Hospitals");

            migrationBuilder.DropTable(
                name: "AdminOfHospitals");

            migrationBuilder.DropTable(
                name: "HospitalClinicLabs");

            migrationBuilder.DropTable(
                name: "HospitalGovernorates");

            migrationBuilder.DropTable(
                name: "UploadedFiles");

            migrationBuilder.DropTable(
                name: "HospitalAdmins");

            migrationBuilder.DropTable(
                name: "Governorates");

            migrationBuilder.DropIndex(
                name: "IX_Hospitals_UploadedFileId",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Hospitals");

            migrationBuilder.DropColumn(
                name: "UploadedFileId",
                table: "Hospitals");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Hospitals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Hospitals",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<string>(
                name: "Government",
                table: "Hospitals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Hospitals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "HospitalId",
                table: "ClinicLabs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClinicLabs_HospitalId",
                table: "ClinicLabs",
                column: "HospitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClinicLabs_Hospitals_HospitalId",
                table: "ClinicLabs",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
