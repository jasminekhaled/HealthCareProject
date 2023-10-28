using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.EF.Migrations
{
    public partial class UserName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Patients",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "Doctors",
                newName: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Patients",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Doctors",
                newName: "FullName");
        }
    }
}
