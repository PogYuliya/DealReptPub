using Microsoft.EntityFrameworkCore.Migrations;

namespace DealRept.Migrations.Identity
{
    public partial class employeeNumberUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_EmployeeNumber",
                table: "AspNetUsers",
                column: "EmployeeNumber");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "29c1a6fd-63ac-4c8e-afc2-245d23bddd51", "f01e7d75-1c9a-400d-adb7-c9d7dc4bc9a9", "ContractStaff", "CONTRACTSTAFF" },
                    { "c9a18f57-3633-4b48-b8fc-b17d4097f53c", "11b460ce-d022-453a-8e99-a6d4e8daa7f0", "Administrator", "ADMINISTRATOR" },
                    { "07f79753-8549-4fb7-a527-1cf878a52d22", "fbeb6eb8-ddd2-4694-8d8e-642838ea13bd", "BranchStaff", "BRANCHSTAFF" },
                    { "f309bcd5-30d9-4697-b89c-f320ec7786f6", "022ca703-b1e8-4e16-9fd7-5c43685c7125", "JustRegistered", "JUSTREGISTERED" },
                    { "4f39fbc6-35f6-4d57-b480-0dd1996dfc49", "fc271436-121d-45ab-945c-75bebc1e5bf4", "Suspended", "SUSPENDED" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_EmployeeNumber",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "07f79753-8549-4fb7-a527-1cf878a52d22");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "29c1a6fd-63ac-4c8e-afc2-245d23bddd51");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4f39fbc6-35f6-4d57-b480-0dd1996dfc49");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c9a18f57-3633-4b48-b8fc-b17d4097f53c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f309bcd5-30d9-4697-b89c-f320ec7786f6");

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
    }
}
