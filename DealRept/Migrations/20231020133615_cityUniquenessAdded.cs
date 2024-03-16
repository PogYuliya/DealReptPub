using Microsoft.EntityFrameworkCore.Migrations;

namespace DealRept.Migrations
{
    public partial class cityUniquenessAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Cities_Name",
                table: "Cities",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Cities_Name",
                table: "Cities");
        }
    }
}
