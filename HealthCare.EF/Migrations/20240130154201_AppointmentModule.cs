using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.EF.Migrations
{
    public partial class AppointmentModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.CreateTable(
                name: "ClinicAppointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClinicLabId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicAppointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicAppointments_ClinicLabs_ClinicLabId",
                        column: x => x.ClinicLabId,
                        principalTable: "ClinicLabs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClinicAppointments_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Days",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Days", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LabAppointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabAppointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabAppointments_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabAppointments_Labs_LabId",
                        column: x => x.LabId,
                        principalTable: "Labs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "XrayAppointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    XrayId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XrayAppointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XrayAppointments_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_XrayAppointments_Xrays_XrayId",
                        column: x => x.XrayId,
                        principalTable: "Xrays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClinicAppointmentDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClinicAppointmentId = table.Column<int>(type: "int", nullable: false),
                    DayId = table.Column<int>(type: "int", nullable: false),
                    FromTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicAppointmentDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicAppointmentDates_ClinicAppointments_ClinicAppointmentId",
                        column: x => x.ClinicAppointmentId,
                        principalTable: "ClinicAppointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClinicAppointmentDates_Days_DayId",
                        column: x => x.DayId,
                        principalTable: "Days",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabAppointmentDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabAppointmentId = table.Column<int>(type: "int", nullable: false),
                    DayId = table.Column<int>(type: "int", nullable: false),
                    FromTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabAppointmentDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabAppointmentDates_Days_DayId",
                        column: x => x.DayId,
                        principalTable: "Days",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabAppointmentDates_LabAppointments_LabAppointmentId",
                        column: x => x.LabAppointmentId,
                        principalTable: "LabAppointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "XrayAppointmentDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    XrayAppointmentId = table.Column<int>(type: "int", nullable: false),
                    DayId = table.Column<int>(type: "int", nullable: false),
                    FromTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToTime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XrayAppointmentDates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XrayAppointmentDates_Days_DayId",
                        column: x => x.DayId,
                        principalTable: "Days",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_XrayAppointmentDates_XrayAppointments_XrayAppointmentId",
                        column: x => x.XrayAppointmentId,
                        principalTable: "XrayAppointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClinicReservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClinicAppointmentDateId = table.Column<int>(type: "int", nullable: false),
                    ClinicAppointmentId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicReservations_ClinicAppointmentDates_ClinicAppointmentDateId",
                        column: x => x.ClinicAppointmentDateId,
                        principalTable: "ClinicAppointmentDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClinicReservations_ClinicAppointments_ClinicAppointmentId",
                        column: x => x.ClinicAppointmentId,
                        principalTable: "ClinicAppointments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClinicReservations_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LabReservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabAppointmentDateId = table.Column<int>(type: "int", nullable: false),
                    LabAppointmentId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabReservations_LabAppointmentDates_LabAppointmentDateId",
                        column: x => x.LabAppointmentDateId,
                        principalTable: "LabAppointmentDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabReservations_LabAppointments_LabAppointmentId",
                        column: x => x.LabAppointmentId,
                        principalTable: "LabAppointments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LabReservations_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "XrayReservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    XrayAppointmentDateId = table.Column<int>(type: "int", nullable: false),
                    XrayAppointmentId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XrayReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XrayReservations_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_XrayReservations_XrayAppointmentDates_XrayAppointmentDateId",
                        column: x => x.XrayAppointmentDateId,
                        principalTable: "XrayAppointmentDates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_XrayReservations_XrayAppointments_XrayAppointmentId",
                        column: x => x.XrayAppointmentId,
                        principalTable: "XrayAppointments",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Days",
                columns: new[] { "Id", "DayName" },
                values: new object[,]
                {
                    { 10, "Saturday" },
                    { 11, "Sunday" },
                    { 12, "Monday" },
                    { 13, "Tuesday" },
                    { 14, "Wednesday" },
                    { 15, "Thursday" },
                    { 16, "Friday" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClinicAppointmentDates_ClinicAppointmentId",
                table: "ClinicAppointmentDates",
                column: "ClinicAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicAppointmentDates_DayId",
                table: "ClinicAppointmentDates",
                column: "DayId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicAppointments_ClinicLabId",
                table: "ClinicAppointments",
                column: "ClinicLabId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicAppointments_DoctorId",
                table: "ClinicAppointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicReservations_ClinicAppointmentDateId",
                table: "ClinicReservations",
                column: "ClinicAppointmentDateId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicReservations_ClinicAppointmentId",
                table: "ClinicReservations",
                column: "ClinicAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicReservations_PatientId",
                table: "ClinicReservations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_LabAppointmentDates_DayId",
                table: "LabAppointmentDates",
                column: "DayId");

            migrationBuilder.CreateIndex(
                name: "IX_LabAppointmentDates_LabAppointmentId",
                table: "LabAppointmentDates",
                column: "LabAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LabAppointments_DoctorId",
                table: "LabAppointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_LabAppointments_LabId",
                table: "LabAppointments",
                column: "LabId");

            migrationBuilder.CreateIndex(
                name: "IX_LabReservations_LabAppointmentDateId",
                table: "LabReservations",
                column: "LabAppointmentDateId");

            migrationBuilder.CreateIndex(
                name: "IX_LabReservations_LabAppointmentId",
                table: "LabReservations",
                column: "LabAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LabReservations_PatientId",
                table: "LabReservations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_XrayAppointmentDates_DayId",
                table: "XrayAppointmentDates",
                column: "DayId");

            migrationBuilder.CreateIndex(
                name: "IX_XrayAppointmentDates_XrayAppointmentId",
                table: "XrayAppointmentDates",
                column: "XrayAppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_XrayAppointments_DoctorId",
                table: "XrayAppointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_XrayAppointments_XrayId",
                table: "XrayAppointments",
                column: "XrayId");

            migrationBuilder.CreateIndex(
                name: "IX_XrayReservations_PatientId",
                table: "XrayReservations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_XrayReservations_XrayAppointmentDateId",
                table: "XrayReservations",
                column: "XrayAppointmentDateId");

            migrationBuilder.CreateIndex(
                name: "IX_XrayReservations_XrayAppointmentId",
                table: "XrayReservations",
                column: "XrayAppointmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClinicReservations");

            migrationBuilder.DropTable(
                name: "LabReservations");

            migrationBuilder.DropTable(
                name: "XrayReservations");

            migrationBuilder.DropTable(
                name: "ClinicAppointmentDates");

            migrationBuilder.DropTable(
                name: "LabAppointmentDates");

            migrationBuilder.DropTable(
                name: "XrayAppointmentDates");

            migrationBuilder.DropTable(
                name: "ClinicAppointments");

            migrationBuilder.DropTable(
                name: "LabAppointments");

            migrationBuilder.DropTable(
                name: "Days");

            migrationBuilder.DropTable(
                name: "XrayAppointments");

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClinicLabId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_ClinicLabs_ClinicLabId",
                        column: x => x.ClinicLabId,
                        principalTable: "ClinicLabs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Appointments_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppointmentId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ClinicLabId",
                table: "Appointments",
                column: "ClinicLabId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_AppointmentId",
                table: "Reservations",
                column: "AppointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_PatientId",
                table: "Reservations",
                column: "PatientId");
        }
    }
}
