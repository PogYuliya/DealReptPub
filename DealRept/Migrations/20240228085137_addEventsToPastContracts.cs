using Microsoft.EntityFrameworkCore.Migrations;

namespace DealRept.Migrations
{
    public partial class addEventsToPastContracts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PastContractID",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_PastContractID",
                table: "Events",
                column: "PastContractID");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_PastContracts_PastContractID",
                table: "Events",
                column: "PastContractID",
                principalTable: "PastContracts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_PastContracts_PastContractID",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_PastContractID",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PastContractID",
                table: "Events");
        }
    }
}
