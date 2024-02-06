using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.EF.Migrations
{
    public partial class bandModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bands_CurrentStateId",
                table: "Bands");

            migrationBuilder.DropColumn(
                name: "HeartBeat",
                table: "CurrentStates");

            migrationBuilder.AlterColumn<int>(
                name: "OxygenLevel",
                table: "CurrentStates",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BodyTemperature",
                table: "CurrentStates",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BloodPressure",
                table: "CurrentStates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HeartRate",
                table: "CurrentStates",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomNum",
                table: "Bands",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Bands",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UniqueId",
                table: "Bands",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SavedBands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BandId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedBands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedBands_Bands_BandId",
                        column: x => x.BandId,
                        principalTable: "Bands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedBands_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bands_CurrentStateId",
                table: "Bands",
                column: "CurrentStateId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SavedBands_BandId",
                table: "SavedBands",
                column: "BandId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedBands_DoctorId",
                table: "SavedBands",
                column: "DoctorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedBands");

            migrationBuilder.DropIndex(
                name: "IX_Bands_CurrentStateId",
                table: "Bands");

            migrationBuilder.DropColumn(
                name: "BloodPressure",
                table: "CurrentStates");

            migrationBuilder.DropColumn(
                name: "HeartRate",
                table: "CurrentStates");

            migrationBuilder.DropColumn(
                name: "RoomNum",
                table: "Bands");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Bands");

            migrationBuilder.DropColumn(
                name: "UniqueId",
                table: "Bands");

            migrationBuilder.AlterColumn<int>(
                name: "OxygenLevel",
                table: "CurrentStates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BodyTemperature",
                table: "CurrentStates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HeartBeat",
                table: "CurrentStates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bands_CurrentStateId",
                table: "Bands",
                column: "CurrentStateId");
        }
    }
}
