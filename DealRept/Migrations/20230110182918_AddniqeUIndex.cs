using Microsoft.EntityFrameworkCore.Migrations;

namespace DealRept.Migrations
{
    public partial class AddniqeUIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Suppliers_LegalCode",
                table: "Suppliers",
                column: "LegalCode");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PastContracts_ContractNumber_ConclusionDate",
                table: "PastContracts",
                columns: new[] { "ContractNumber", "ConclusionDate" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CurrentContracts_ContractNumber_ConclusionDate",
                table: "CurrentContracts",
                columns: new[] { "ContractNumber", "ConclusionDate" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Branches_Name",
                table: "Branches",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Banks_Code",
                table: "Banks",
                column: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Suppliers_LegalCode",
                table: "Suppliers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_PastContracts_ContractNumber_ConclusionDate",
                table: "PastContracts");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CurrentContracts_ContractNumber_ConclusionDate",
                table: "CurrentContracts");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Branches_Name",
                table: "Branches");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Banks_Code",
                table: "Banks");
        }
    }
}
