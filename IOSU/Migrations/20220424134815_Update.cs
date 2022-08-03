using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IOSU.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contract_Manager_ManagerPassportNumber",
                table: "Contract");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contract",
                table: "Contract");

            migrationBuilder.AlterColumn<string>(
                name: "ManagerPassportNumber",
                table: "Contract",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Contract",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contract",
                table: "Contract",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_ManagerPassportNumber",
                table: "Contract",
                column: "ManagerPassportNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_Manager_ManagerPassportNumber",
                table: "Contract",
                column: "ManagerPassportNumber",
                principalTable: "Manager",
                principalColumn: "PassportNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contract_Manager_ManagerPassportNumber",
                table: "Contract");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Contract",
                table: "Contract");

            migrationBuilder.DropIndex(
                name: "IX_Contract_ManagerPassportNumber",
                table: "Contract");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Contract");

            migrationBuilder.AlterColumn<string>(
                name: "ManagerPassportNumber",
                table: "Contract",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Contract",
                table: "Contract",
                columns: new[] { "ManagerPassportNumber", "ProductId", "ClientId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Contract_Manager_ManagerPassportNumber",
                table: "Contract",
                column: "ManagerPassportNumber",
                principalTable: "Manager",
                principalColumn: "PassportNumber",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
