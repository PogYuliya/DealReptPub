using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DealRept.Migrations.Identity
{
    public partial class addedRegistrationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "66612b9c-b66b-455b-9d2b-d0f37a7af7e7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1339fc2-cd79-4b1f-af0e-787fc60eb333");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3df0079-8fb5-47d3-98af-6868d5b4902c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b6dccac9-640f-4290-9583-74aa3639b743");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb7f9839-85f8-474f-999b-44ed5ddd51b3");

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
                    { "762919a1-1647-496f-b6e2-a9cf3d40925f", "0fd80c9c-ee21-422b-a0b3-7f19b312734e", "ContractStaff", "CONTRACTSTAFF" },
                    { "207a6677-5968-4e92-90d0-2787f7a0093b", "9a394f22-e4b6-4127-a445-f400dd065088", "Administrator", "ADMINISTRATOR" },
                    { "dd1d721e-dd0f-412b-828b-4efc47287a28", "836ad27b-bb32-4e67-bb6c-036ca193c29e", "BranchStaff", "BRANCHSTAFF" },
                    { "969f1b27-f295-4953-b97a-f7860d643312", "93881dd4-ed8d-49eb-82bc-7ad0e7991dc0", "JustRegistered", "JUSTREGISTERED" },
                    { "d22ee507-087f-47c2-868c-bf7b92c4d616", "eb14c1d4-bfe7-44de-a7de-e6cbb52af806", "Suspended", "SUSPENDED" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "207a6677-5968-4e92-90d0-2787f7a0093b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "762919a1-1647-496f-b6e2-a9cf3d40925f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "969f1b27-f295-4953-b97a-f7860d643312");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d22ee507-087f-47c2-868c-bf7b92c4d616");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dd1d721e-dd0f-412b-828b-4efc47287a28");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b6dccac9-640f-4290-9583-74aa3639b743", "3cc44424-ae65-43e6-8ffb-43f34f5a90cb", "ContractStaff", "CONTRACTSTAFF" },
                    { "b3df0079-8fb5-47d3-98af-6868d5b4902c", "651f5778-97a9-4d45-acdd-92968d804b0e", "Administrator", "ADMINISTRATOR" },
                    { "cb7f9839-85f8-474f-999b-44ed5ddd51b3", "0bc62864-6caa-4388-8929-494d9835173e", "BranchStaff", "BRANCHSTAFF" },
                    { "a1339fc2-cd79-4b1f-af0e-787fc60eb333", "51e7f070-f967-4ae1-92aa-6d05834d61df", "JustRegistered", "JUSTREGISTERED" },
                    { "66612b9c-b66b-455b-9d2b-d0f37a7af7e7", "1a9ca471-a60a-4f0a-b1a5-72210b332e3f", "Suspended", "SUSPENDED" }
                });
        }
    }
}
