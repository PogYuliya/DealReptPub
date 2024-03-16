using Microsoft.EntityFrameworkCore.Migrations;

namespace DealRept.Migrations
{
    public partial class AdBranchCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Branches_Name",
                table: "Branches");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Branches",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Branches_Code",
                table: "Branches",
                column: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Branches_Code",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Branches");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Branches_Name",
                table: "Branches",
                column: "Name");
        }
    }
}
