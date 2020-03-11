using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectComite.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gemeente",
                columns: table => new
                {
                    gemeenteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    naam = table.Column<string>(nullable: true),
                    postcode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gemeente", x => x.gemeenteId);
                });

            migrationBuilder.CreateTable(
                name: "Actie",
                columns: table => new
                {
                    actieId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    informatie = table.Column<string>(nullable: true),
                    GemeenteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actie", x => x.actieId);
                    table.ForeignKey(
                        name: "FK_Actie_Gemeente_GemeenteId",
                        column: x => x.GemeenteId,
                        principalTable: "Gemeente",
                        principalColumn: "gemeenteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lid",
                columns: table => new
                {
                    lidId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    naam = table.Column<string>(nullable: true),
                    gemeenteId = table.Column<int>(nullable: false),
                    lidgeldBetaald = table.Column<bool>(nullable: false),
                    emailAdres = table.Column<string>(nullable: true),
                    telefoonnummer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lid", x => x.lidId);
                    table.ForeignKey(
                        name: "FK_Lid_Gemeente_gemeenteId",
                        column: x => x.gemeenteId,
                        principalTable: "Gemeente",
                        principalColumn: "gemeenteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActieLid",
                columns: table => new
                {
                    actieLidId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    actieId = table.Column<int>(nullable: false),
                    lidId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActieLid", x => x.actieLidId);
                    table.ForeignKey(
                        name: "FK_ActieLid_Actie_actieId",
                        column: x => x.actieId,
                        principalTable: "Actie",
                        principalColumn: "actieId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActieLid_Lid_lidId",
                        column: x => x.lidId,
                        principalTable: "Lid",
                        principalColumn: "lidId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actie_GemeenteId",
                table: "Actie",
                column: "GemeenteId");

            migrationBuilder.CreateIndex(
                name: "IX_ActieLid_actieId",
                table: "ActieLid",
                column: "actieId");

            migrationBuilder.CreateIndex(
                name: "IX_ActieLid_lidId",
                table: "ActieLid",
                column: "lidId");

            migrationBuilder.CreateIndex(
                name: "IX_Lid_gemeenteId",
                table: "Lid",
                column: "gemeenteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActieLid");

            migrationBuilder.DropTable(
                name: "Actie");

            migrationBuilder.DropTable(
                name: "Lid");

            migrationBuilder.DropTable(
                name: "Gemeente");
        }
    }
}
