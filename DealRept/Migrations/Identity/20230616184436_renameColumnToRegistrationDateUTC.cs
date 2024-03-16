using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DealRept.Migrations.Identity
{
    public partial class renameColumnToRegistrationDateUTC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "344c4438-435c-484d-a046-78c26f5420ad");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7875c06c-c194-4dd1-9369-8ac75bc8b43c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83e30c84-1b12-4ebb-bdc4-b96ae4549c9f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3554651-4f0a-4615-8537-9802f7447c55");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f58a1fca-f621-48a7-baa5-128d740d2737");

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDateUTC",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "RegistrationDateUTC",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7875c06c-c194-4dd1-9369-8ac75bc8b43c", "022c2b18-965d-451e-ac96-be886803c49c", "ContractStaff", "CONTRACTSTAFF" },
                    { "f3554651-4f0a-4615-8537-9802f7447c55", "f3c82cf3-b530-48d0-bdac-73b773859a8d", "Administrator", "ADMINISTRATOR" },
                    { "344c4438-435c-484d-a046-78c26f5420ad", "ebe17614-7ec9-42cf-bf86-f7f97d050076", "BranchStaff", "BRANCHSTAFF" },
                    { "83e30c84-1b12-4ebb-bdc4-b96ae4549c9f", "33620093-c553-4d4a-848a-fd5f64726439", "JustRegistered", "JUSTREGISTERED" },
                    { "f58a1fca-f621-48a7-baa5-128d740d2737", "b1369fbf-369f-4289-a94e-96b619a80b19", "Suspended", "SUSPENDED" }
                });
        }
    }
}
