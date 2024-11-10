using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptiArroz_Tesis_Proyect.Migrations
{
    public partial class AddRelationLotZone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdZone",
                table: "RiceLots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RiceLots_IdZone",
                table: "RiceLots",
                column: "IdZone");

            migrationBuilder.AddForeignKey(
                name: "FK_RiceLots_Zones_IdZone",
                table: "RiceLots",
                column: "IdZone",
                principalTable: "Zones",
                principalColumn: "IdZone",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiceLots_Zones_IdZone",
                table: "RiceLots");

            migrationBuilder.DropIndex(
                name: "IX_RiceLots_IdZone",
                table: "RiceLots");

            migrationBuilder.DropColumn(
                name: "IdZone",
                table: "RiceLots");
        }
    }
}
