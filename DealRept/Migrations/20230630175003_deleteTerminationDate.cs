using Microsoft.EntityFrameworkCore.Migrations;

namespace DealRept.Migrations
{
    public partial class deleteTerminationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CHK_TermDate",
                table: "CurrentContracts");

            migrationBuilder.AddCheckConstraint(
                name: "CHK_TermDate",
                table: "CurrentContracts",
                sql: "ExpirationDate > ConclusionDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CHK_TermDate",
                table: "CurrentContracts");

            migrationBuilder.AddCheckConstraint(
                name: "CHK_TermDate",
                table: "CurrentContracts",
                sql: "TerminationDate > ConclusionDate");
        }
    }
}
