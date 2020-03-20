using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectComite.Migrations
{
    public partial class Migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ActieLid",
                table: "ActieLid");

            migrationBuilder.DropIndex(
                name: "IX_ActieLid_actieId",
                table: "ActieLid");

            migrationBuilder.AlterColumn<int>(
                name: "actieLidId",
                table: "ActieLid",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActieLid",
                table: "ActieLid",
                columns: new[] { "actieId", "lidId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ActieLid",
                table: "ActieLid");

            migrationBuilder.AlterColumn<int>(
                name: "actieLidId",
                table: "ActieLid",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActieLid",
                table: "ActieLid",
                column: "actieLidId");

            migrationBuilder.CreateIndex(
                name: "IX_ActieLid_actieId",
                table: "ActieLid",
                column: "actieId");
        }
    }
}
