using Microsoft.EntityFrameworkCore.Migrations;

namespace DealRept.Migrations
{
    public partial class isdeletedAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Cities_Name",
                table: "Cities");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PastDocuments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CurrentDocuments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PastDocuments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CurrentDocuments");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Cities_Name",
                table: "Cities",
                column: "Name");
        }
    }
}
