using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.EF.Migrations
{
    public partial class bandState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OxygenLevel",
                table: "CurrentStates",
                newName: "Temperature");

            migrationBuilder.RenameColumn(
                name: "BodyTemperature",
                table: "CurrentStates",
                newName: "Oxygen");

            migrationBuilder.AddColumn<int>(
                name: "BloodSugar",
                table: "CurrentStates",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BloodSugar",
                table: "CurrentStates");

            migrationBuilder.RenameColumn(
                name: "Temperature",
                table: "CurrentStates",
                newName: "OxygenLevel");

            migrationBuilder.RenameColumn(
                name: "Oxygen",
                table: "CurrentStates",
                newName: "BodyTemperature");
        }
    }
}
