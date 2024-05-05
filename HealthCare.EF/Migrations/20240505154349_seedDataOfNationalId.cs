using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthCare.EF.Migrations
{
    public partial class seedDataOfNationalId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
               table: "CivilRegestrations",
               columns: new[] { "Id", "NationalId", "Name" },
               values: new object[] { 30, "14141414141414", "جاسمين خالد احمد" }
           );

            migrationBuilder.InsertData(
               table: "CivilRegestrations",
               columns: new[] { "Id", "NationalId", "Name" },
               values: new object[] { 31, "13131313131313", "احمد محمد متولي" }
           );

            migrationBuilder.InsertData(
               table: "CivilRegestrations",
               columns: new[] { "Id", "NationalId", "Name" },
               values: new object[] { 32, "12121212121212", "كيرلس صبري كيرلس" }
           );

            migrationBuilder.InsertData(
               table: "CivilRegestrations",
               columns: new[] { "Id", "NationalId", "Name" },
               values: new object[] { 33, "00000000000000", "شروق اشرف عبد الفتاح" }
           );

            migrationBuilder.InsertData(
               table: "CivilRegestrations",
               columns: new[] { "Id", "NationalId", "Name" },
               values: new object[] { 34, "99999999999999", "محمد علي ياسين" }
           );


            migrationBuilder.InsertData(
               table: "CivilRegestrations",
               columns: new[] { "Id", "NationalId", "Name" },
               values: new object[] { 35, "88888888888888", "يونس احمد محمد" }
           );

            migrationBuilder.InsertData(
               table: "CivilRegestrations",
               columns: new[] { "Id", "NationalId", "Name" },
               values: new object[] { 36, "77777777777777", "مراد محمد علي" }
           );

            migrationBuilder.InsertData(
               table: "CivilRegestrations",
               columns: new[] { "Id", "NationalId", "Name" },
               values: new object[] { 37, "11111111111111", "حسام محمد احمد" }
           );


            migrationBuilder.InsertData(
               table: "CivilRegestrations",
               columns: new[] { "Id", "NationalId", "Name" },
               values: new object[] { 38, "22222222222222", "عبدالرحمن نور محمود" }
           );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [CivilRegestrations]");
        }
    }
}
