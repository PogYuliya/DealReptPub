using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace DealRept.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "char(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ContractStatuses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactStatuses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Specializations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specializations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    StreetBuilding = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PostalIndex = table.Column<string>(type: "char(5)", maxLength: 5, nullable: false),
                    HeadFirstName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    HeadLastName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    HeadMiddleName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    HeadTelephone = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    HeadEmail = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    CityID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Branches_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    StreetBuilding = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    PostalIndex = table.Column<string>(type: "char(5)", maxLength: 5, nullable: false),
                    LegalCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    BankAccount = table.Column<string>(type: "varchar(29)", maxLength: 29, nullable: false),
                    CompanyTelephone = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    CompanyEmail = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    ContactFirstName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ContactLastName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ContactMiddleName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ContactTelephone = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true),
                    ContactEmail = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    DirectorFirstName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    DirectorLastName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    DirectorMiddleName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    NegativeRemarks = table.Column<string>(type: "text", nullable: true),
                    CityID = table.Column<int>(type: "int", nullable: false),
                    BankID = table.Column<int>(type: "int", nullable: false),
                    SpecializationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Suppliers_Banks_BankID",
                        column: x => x.BankID,
                        principalTable: "Banks",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Suppliers_Cities_CityID",
                        column: x => x.CityID,
                        principalTable: "Cities",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Suppliers_Specializations_SpecializationID",
                        column: x => x.SpecializationID,
                        principalTable: "Specializations",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CurrentContracts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ContractNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    ConclusionDate = table.Column<DateTime>(type: "Date", nullable: false),
                    TerminationDate = table.Column<DateTime>(type: "Date", nullable: false),
                    Subject = table.Column<string>(type: "Text", nullable: false),
                    ContractAmount = table.Column<decimal>(type: "Decimal(10,2)", nullable: false),
                    IsProlonged = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    IsGoods = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    SupplierID = table.Column<int>(type: "int", nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: false),
                    ContractStatusID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentContracts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CurrentContracts_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CurrentContracts_ContactStatuses_ContractStatusID",
                        column: x => x.ContractStatusID,
                        principalTable: "ContractStatuses",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_CurrentContracts_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PastContracts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ContractNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    ConclusionDate = table.Column<DateTime>(type: "Date", nullable: false),
                    TerminationDate = table.Column<DateTime>(type: "Date", nullable: false),
                    TransitionDate = table.Column<DateTime>(type: "Date", nullable: false),
                    Subject = table.Column<string>(type: "Text", nullable: false),
                    ContractAmount = table.Column<decimal>(type: "Decimal(10,2)", nullable: false),
                    IsProlonged = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsGoods = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Remarks = table.Column<string>(type: "text", nullable: true),
                    SupplierID = table.Column<int>(type: "int", nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: false),
                    ContractStatusID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PastContracts", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PastContracts_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PastContracts_ContactStatuses_ContractStatusID",
                        column: x => x.ContractStatusID,
                        principalTable: "ContractStatuses",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PastContracts_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CurrentDocuments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    PathToDocument = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    DateOfUploading = table.Column<DateTime>(type: "Date", nullable: false),
                    CurrentContractID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentDocuments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CurrentDocuments_CurrentContracts_CurrentContractID",
                        column: x => x.CurrentContractID,
                        principalTable: "CurrentContracts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PastDocuments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    PathToDocument = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    DateOfUploading = table.Column<DateTime>(type: "Date", nullable: false),
                    PastContractID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PastDocuments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PastDocuments_PastContracts_PastContractID",
                        column: x => x.PastContractID,
                        principalTable: "PastContracts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CityID",
                table: "Branches",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentContracts_BranchID",
                table: "CurrentContracts",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentContracts_ContractStatusID",
                table: "CurrentContracts",
                column: "ContractStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentContracts_SupplierID",
                table: "CurrentContracts",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentDocuments_CurrentContractID",
                table: "CurrentDocuments",
                column: "CurrentContractID");

            migrationBuilder.CreateIndex(
                name: "IX_PastContracts_BranchID",
                table: "PastContracts",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_PastContracts_ContractStatusID",
                table: "PastContracts",
                column: "ContractStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_PastContracts_SupplierID",
                table: "PastContracts",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_PastDocuments_PastContractID",
                table: "PastDocuments",
                column: "PastContractID");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_BankID",
                table: "Suppliers",
                column: "BankID");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CityID",
                table: "Suppliers",
                column: "CityID");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_SpecializationID",
                table: "Suppliers",
                column: "SpecializationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrentDocuments");

            migrationBuilder.DropTable(
                name: "PastDocuments");

            migrationBuilder.DropTable(
                name: "CurrentContracts");

            migrationBuilder.DropTable(
                name: "PastContracts");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "ContractStatuses");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Specializations");
        }
    }
}
