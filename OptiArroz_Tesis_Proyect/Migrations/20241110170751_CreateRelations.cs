using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptiArroz_Tesis_Proyect.Migrations
{
    public partial class CreateRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<int>(
                name: "IdZone",
                table: "RiceLots",
                type: "int",
                nullable: false,
                defaultValue: 0);



            migrationBuilder.AddColumn<int>(
                name: "IdZoneDestination",
                table: "RiceLotMovements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdZoneOrigin",
                table: "RiceLotMovements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RiceLots_IdZone",
                table: "RiceLots",
                column: "IdZone");

            migrationBuilder.CreateIndex(
                name: "IX_RiceLotMovements_IdZoneDestination",
                table: "RiceLotMovements",
                column: "IdZoneDestination");

            migrationBuilder.CreateIndex(
                name: "IX_RiceLotMovements_IdZoneOrigin",
                table: "RiceLotMovements",
                column: "IdZoneOrigin");


            migrationBuilder.AddForeignKey(
                name: "FK_RiceLotMovements_Zones_IdZoneDestination",
                table: "RiceLotMovements",
                column: "IdZoneDestination",
                principalTable: "Zones",
                principalColumn: "IdZone",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiceLotMovements_Zones_IdZoneOrigin",
                table: "RiceLotMovements",
                column: "IdZoneOrigin",
                principalTable: "Zones",
                principalColumn: "IdZone",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_RiceLotMovements_Ubications_IdDestination",
                table: "RiceLotMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_RiceLotMovements_Ubications_IdOrigin",
                table: "RiceLotMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_RiceLotMovements_Zones_IdZoneDestination",
                table: "RiceLotMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_RiceLotMovements_Zones_IdZoneOrigin",
                table: "RiceLotMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_RiceLots_Zones_IdZone",
                table: "RiceLots");

            migrationBuilder.DropIndex(
                name: "IX_RiceLots_IdZone",
                table: "RiceLots");

            migrationBuilder.DropIndex(
                name: "IX_RiceLotMovements_IdZoneDestination",
                table: "RiceLotMovements");

            migrationBuilder.DropIndex(
                name: "IX_RiceLotMovements_IdZoneOrigin",
                table: "RiceLotMovements");

            migrationBuilder.DropColumn(
                name: "IdZone",
                table: "RiceLots");

            migrationBuilder.DropColumn(
                name: "IdZoneDestination",
                table: "RiceLotMovements");

            migrationBuilder.DropColumn(
                name: "IdZoneOrigin",
                table: "RiceLotMovements");

            migrationBuilder.AlterColumn<int>(
                name: "IdOrigin",
                table: "RiceLotMovements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdDestination",
                table: "RiceLotMovements",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RiceLotMovements_Ubications_IdDestination",
                table: "RiceLotMovements",
                column: "IdDestination",
                principalTable: "Ubications",
                principalColumn: "IdUbication",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RiceLotMovements_Ubications_IdOrigin",
                table: "RiceLotMovements",
                column: "IdOrigin",
                principalTable: "Ubications",
                principalColumn: "IdUbication",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
