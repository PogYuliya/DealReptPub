using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace DealRept.Migrations
{
    public partial class addCurrentEventsAndPastEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.CreateTable(
                name: "CurrentEvents",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    CurrentContractID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    Start = table.Column<DateTime>(type: "DATETIME ", nullable: false),
                    End = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Description = table.Column<string>(type: "Text", maxLength: 500, nullable: false),
                    AllDay = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentEvents", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CurrentEvents_CurrentContracts_CurrentContractID",
                        column: x => x.CurrentContractID,
                        principalTable: "CurrentContracts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PastEvents",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    PastContractID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    Start = table.Column<DateTime>(type: "DATETIME ", nullable: false),
                    End = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Description = table.Column<string>(type: "Text", maxLength: 500, nullable: false),
                    AllDay = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PastEvents", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PastEvents_PastContracts_PastContractID",
                        column: x => x.PastContractID,
                        principalTable: "PastContracts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrentEvents_CurrentContractID",
                table: "CurrentEvents",
                column: "CurrentContractID");

            migrationBuilder.CreateIndex(
                name: "IX_PastEvents_PastContractID",
                table: "PastEvents",
                column: "PastContractID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrentEvents");

            migrationBuilder.DropTable(
                name: "PastEvents");

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AllDay = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ContractID = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "Text", maxLength: 500, nullable: false),
                    End = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    PastContractID = table.Column<int>(type: "int", nullable: true),
                    Start = table.Column<DateTime>(type: "DATETIME ", nullable: false),
                    Title = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Events_CurrentContracts_ContractID",
                        column: x => x.ContractID,
                        principalTable: "CurrentContracts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Events_PastContracts_PastContractID",
                        column: x => x.PastContractID,
                        principalTable: "PastContracts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_ContractID",
                table: "Events",
                column: "ContractID");

            migrationBuilder.CreateIndex(
                name: "IX_Events_PastContractID",
                table: "Events",
                column: "PastContractID");
        }
    }
}
