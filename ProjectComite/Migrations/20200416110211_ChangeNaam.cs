using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectComite.Migrations
{
    public partial class ChangeNaam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Naaam",
                table: "AspNetUsers",
                newName: "Naam");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Naam",
                table: "AspNetUsers",
                newName: "Naaam");
        }
    }
}
