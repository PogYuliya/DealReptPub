using Microsoft.EntityFrameworkCore.Migrations;

namespace DealRept.Migrations
{
    public partial class changedPastContactTransDateConstaraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CHK_TransDate",
                table: "PastContracts");

            migrationBuilder.AddCheckConstraint(
                name: "CHK_TransDate",
                table: "PastContracts",
                sql: "TransitionDate >= ConclusionDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CHK_TransDate",
                table: "PastContracts");

            migrationBuilder.AddCheckConstraint(
                name: "CHK_TransDate",
                table: "PastContracts",
                sql: "TransitionDate > ConclusionDate");
        }
    }
}
