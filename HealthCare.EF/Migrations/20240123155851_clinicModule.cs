using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.EF.Migrations
{
    public partial class clinicModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HospitalClinicLabs");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ClinicLabs");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "ClinicLabs");

            migrationBuilder.AddColumn<int>(
                name: "UploadedFileId",
                table: "Specializations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HospitalId",
                table: "ClinicLabs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SpecializationId",
                table: "ClinicLabs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "XraySpecializations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedFileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XraySpecializations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XraySpecializations_UploadedFiles_UploadedFileId",
                        column: x => x.UploadedFileId,
                        principalTable: "UploadedFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Xrays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    XraySpecializationId = table.Column<int>(type: "int", nullable: false),
                    HospitalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Xrays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Xrays_Hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Xrays_XraySpecializations_XraySpecializationId",
                        column: x => x.XraySpecializationId,
                        principalTable: "XraySpecializations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "XrayDoctors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    XrayId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XrayDoctors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XrayDoctors_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_XrayDoctors_Xrays_XrayId",
                        column: x => x.XrayId,
                        principalTable: "Xrays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_UploadedFileId",
                table: "Specializations",
                column: "UploadedFileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClinicLabs_HospitalId",
                table: "ClinicLabs",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicLabs_SpecializationId",
                table: "ClinicLabs",
                column: "SpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_XrayDoctors_DoctorId",
                table: "XrayDoctors",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_XrayDoctors_XrayId",
                table: "XrayDoctors",
                column: "XrayId");

            migrationBuilder.CreateIndex(
                name: "IX_Xrays_HospitalId",
                table: "Xrays",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Xrays_XraySpecializationId",
                table: "Xrays",
                column: "XraySpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_XraySpecializations_UploadedFileId",
                table: "XraySpecializations",
                column: "UploadedFileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClinicLabs_Hospitals_HospitalId",
                table: "ClinicLabs",
                column: "HospitalId",
                principalTable: "Hospitals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClinicLabs_Specializations_SpecializationId",
                table: "ClinicLabs",
                column: "SpecializationId",
                principalTable: "Specializations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Specializations_UploadedFiles_UploadedFileId",
                table: "Specializations",
                column: "UploadedFileId",
                principalTable: "UploadedFiles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClinicLabs_Hospitals_HospitalId",
                table: "ClinicLabs");

            migrationBuilder.DropForeignKey(
                name: "FK_ClinicLabs_Specializations_SpecializationId",
                table: "ClinicLabs");

            migrationBuilder.DropForeignKey(
                name: "FK_Specializations_UploadedFiles_UploadedFileId",
                table: "Specializations");

            migrationBuilder.DropTable(
                name: "XrayDoctors");

            migrationBuilder.DropTable(
                name: "Xrays");

            migrationBuilder.DropTable(
                name: "XraySpecializations");

            migrationBuilder.DropIndex(
                name: "IX_Specializations_UploadedFileId",
                table: "Specializations");

            migrationBuilder.DropIndex(
                name: "IX_ClinicLabs_HospitalId",
                table: "ClinicLabs");

            migrationBuilder.DropIndex(
                name: "IX_ClinicLabs_SpecializationId",
                table: "ClinicLabs");

            migrationBuilder.DropColumn(
                name: "UploadedFileId",
                table: "Specializations");

            migrationBuilder.DropColumn(
                name: "HospitalId",
                table: "ClinicLabs");

            migrationBuilder.DropColumn(
                name: "SpecializationId",
                table: "ClinicLabs");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ClinicLabs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "ClinicLabs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "HospitalClinicLabs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClinicLabId = table.Column<int>(type: "int", nullable: false),
                    HospitalId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_HospitalClinicLabs_ClinicLabId",
                table: "HospitalClinicLabs",
                column: "ClinicLabId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HospitalClinicLabs_HospitalId",
                table: "HospitalClinicLabs",
                column: "HospitalId");
        }
    }
}
