using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectComite.Migrations
{
    public partial class AddActieNaam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Naam",
                table: "Actie",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Naam",
                table: "Actie");
        }
    }
}
