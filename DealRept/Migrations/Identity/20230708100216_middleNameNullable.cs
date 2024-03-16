using Microsoft.EntityFrameworkCore.Migrations;

namespace DealRept.Migrations.Identity
{
    public partial class middleNameNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                table: "AspNetUsers",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b79778c9-1bd8-45fc-b079-4a511b37590e", "dbb97d3c-7e16-439e-8b56-bf77acda5104", "ContractStaff", "CONTRACTSTAFF" },
                    { "1ebac553-5635-40ae-b6aa-bb5068728dad", "233b4214-6100-4442-b6e7-abff61cb0ba3", "Administrator", "ADMINISTRATOR" },
                    { "7d62fb85-b689-48b4-b333-b2ca079e7f63", "d3a34197-cd2f-4a61-866c-5792789d24c4", "BranchStaff", "BRANCHSTAFF" },
                    { "9a3bfe36-cdec-480e-832e-71819e3e5743", "e8137f57-df95-4a58-8df9-0d38dee73844", "JustRegistered", "JUSTREGISTERED" },
                    { "18884ef6-b6e4-41b7-ad55-9b59977f1200", "84ae9fb0-f1fc-4a6a-91ba-8410655b958f", "Suspended", "SUSPENDED" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "18884ef6-b6e4-41b7-ad55-9b59977f1200");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1ebac553-5635-40ae-b6aa-bb5068728dad");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7d62fb85-b689-48b4-b333-b2ca079e7f63");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a3bfe36-cdec-480e-832e-71819e3e5743");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b79778c9-1bd8-45fc-b079-4a511b37590e");

            migrationBuilder.AlterColumn<string>(
                name: "MiddleName",
                table: "AspNetUsers",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

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
    }
}
