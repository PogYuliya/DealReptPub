using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DealRept.Migrations
{
    public partial class DeleteTransitionDateFromCurrentCon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransitionDate",
                table: "CurrentContracts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TransitionDate",
                table: "CurrentContracts",
                type: "Date",
                nullable: true);
        }
    }
}
