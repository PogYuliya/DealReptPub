using Microsoft.EntityFrameworkCore.Migrations;

namespace DealRept.Migrations
{
    public partial class AddUniqueKeyDocumument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_CurrentDocuments_Name",
                table: "CurrentDocuments");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CurrentDocuments_Name_CurrentContractID",
                table: "CurrentDocuments",
                columns: new[] { "Name", "CurrentContractID" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_CurrentDocuments_Name_CurrentContractID",
                table: "CurrentDocuments");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CurrentDocuments_Name",
                table: "CurrentDocuments",
                column: "Name");
        }
    }
}
