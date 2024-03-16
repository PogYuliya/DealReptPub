using Microsoft.EntityFrameworkCore.Migrations;

namespace DealRept.Migrations
{
    public partial class checkBeforePublishing1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Cities_Name",
                table: "Cities",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Banks_Name",
                table: "Banks",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Cities_Name",
                table: "Cities");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Banks_Name",
                table: "Banks");
        }
    }
}
