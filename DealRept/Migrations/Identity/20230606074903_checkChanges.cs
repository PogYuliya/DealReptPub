using Microsoft.EntityFrameworkCore.Migrations;

namespace DealRept.Migrations.Identity
{
    public partial class checkChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    { "7875c06c-c194-4dd1-9369-8ac75bc8b43c", "022c2b18-965d-451e-ac96-be886803c49c", "ContractStaff", "CONTRACTSTAFF" },
                    { "f3554651-4f0a-4615-8537-9802f7447c55", "f3c82cf3-b530-48d0-bdac-73b773859a8d", "Administrator", "ADMINISTRATOR" },
                    { "344c4438-435c-484d-a046-78c26f5420ad", "ebe17614-7ec9-42cf-bf86-f7f97d050076", "BranchStaff", "BRANCHSTAFF" },
                    { "83e30c84-1b12-4ebb-bdc4-b96ae4549c9f", "33620093-c553-4d4a-848a-fd5f64726439", "JustRegistered", "JUSTREGISTERED" },
                    { "f58a1fca-f621-48a7-baa5-128d740d2737", "b1369fbf-369f-4289-a94e-96b619a80b19", "Suspended", "SUSPENDED" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
