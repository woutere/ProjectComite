using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectComite.Migrations
{
    public partial class ManytoGemeente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actie_Gemeente_GemeenteId",
                table: "Actie");

            migrationBuilder.RenameColumn(
                name: "GemeenteId",
                table: "Actie",
                newName: "gemeenteId");

            migrationBuilder.RenameIndex(
                name: "IX_Actie_GemeenteId",
                table: "Actie",
                newName: "IX_Actie_gemeenteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actie_Gemeente_gemeenteId",
                table: "Actie",
                column: "gemeenteId",
                principalTable: "Gemeente",
                principalColumn: "gemeenteId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actie_Gemeente_gemeenteId",
                table: "Actie");

            migrationBuilder.RenameColumn(
                name: "gemeenteId",
                table: "Actie",
                newName: "GemeenteId");

            migrationBuilder.RenameIndex(
                name: "IX_Actie_gemeenteId",
                table: "Actie",
                newName: "IX_Actie_GemeenteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actie_Gemeente_GemeenteId",
                table: "Actie",
                column: "GemeenteId",
                principalTable: "Gemeente",
                principalColumn: "gemeenteId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
