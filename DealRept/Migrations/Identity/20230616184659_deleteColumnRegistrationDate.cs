using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DealRept.Migrations.Identity
{
    public partial class deleteColumnRegistrationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "36019153-8c5b-4bf3-a6a1-943d7fecc23b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "40d38972-d963-4fb5-8ede-fd80d9346e3e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "933bea26-340b-4bd3-b589-d810a43317a7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e564c08f-3f38-4809-af15-36b1a4244afd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f71a3170-7a3f-4018-a6a4-68c4567c8ff0");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "306726b5-c7b7-4e56-b4da-702afe744747", "7183c8cd-d189-4301-a0af-f760d895e541", "ContractStaff", "CONTRACTSTAFF" },
                    { "dfbeae56-36cf-4102-a252-c9ac94a1d874", "82bcbd28-7e70-41bb-af5a-57498047e3af", "Administrator", "ADMINISTRATOR" },
                    { "5fbbf228-bb44-41f5-8680-4626a08dd49d", "7a85a1ec-8c96-4612-a427-7505c0f9ac40", "BranchStaff", "BRANCHSTAFF" },
                    { "f0a8376f-c071-47c7-bfdb-aee7c050954b", "4bf529f7-2ecd-4286-89e2-43fb292d0a5c", "JustRegistered", "JUSTREGISTERED" },
                    { "592329c4-5d13-4d6e-a168-f6fd50f5791d", "e628805e-56d3-4175-b513-ca620f5c2153", "Suspended", "SUSPENDED" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "306726b5-c7b7-4e56-b4da-702afe744747");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "592329c4-5d13-4d6e-a168-f6fd50f5791d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5fbbf228-bb44-41f5-8680-4626a08dd49d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dfbeae56-36cf-4102-a252-c9ac94a1d874");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f0a8376f-c071-47c7-bfdb-aee7c050954b");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "AspNetUsers",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "933bea26-340b-4bd3-b589-d810a43317a7", "c22c00a2-6309-4610-a3ea-086821c11856", "ContractStaff", "CONTRACTSTAFF" },
                    { "40d38972-d963-4fb5-8ede-fd80d9346e3e", "add62d32-b7d5-4e20-b385-fe84609db6d0", "Administrator", "ADMINISTRATOR" },
                    { "e564c08f-3f38-4809-af15-36b1a4244afd", "1ef31116-8bcc-4074-8927-665c25f8ede2", "BranchStaff", "BRANCHSTAFF" },
                    { "f71a3170-7a3f-4018-a6a4-68c4567c8ff0", "068b409b-9cf3-4f48-904d-179092a9525b", "JustRegistered", "JUSTREGISTERED" },
                    { "36019153-8c5b-4bf3-a6a1-943d7fecc23b", "1d7c847e-0e31-45c6-8e76-2c5ba39d8d9f", "Suspended", "SUSPENDED" }
                });
        }
    }
}
