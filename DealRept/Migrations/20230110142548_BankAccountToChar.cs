using Microsoft.EntityFrameworkCore.Migrations;

namespace DealRept.Migrations
{
    public partial class BankAccountToChar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BankAccount",
                table: "Suppliers",
                type: "char(29)",
                maxLength: 29,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(29)",
                oldMaxLength: 29);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BankAccount",
                table: "Suppliers",
                type: "varchar(29)",
                maxLength: 29,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(29)",
                oldMaxLength: 29);
        }
    }
}
