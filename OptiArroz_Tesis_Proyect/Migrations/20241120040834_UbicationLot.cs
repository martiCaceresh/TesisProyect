using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OptiArroz_Tesis_Proyect.Migrations
{
    public partial class UbicationLot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiceLots_Ubications_IdLastUbication",
                table: "RiceLots");

            migrationBuilder.DropIndex(
                name: "IX_RiceLots_IdLastUbication",
                table: "RiceLots");


            migrationBuilder.CreateIndex(
                name: "IX_RiceLots_IdLastUbication",
                table: "RiceLots",
                column: "IdLastUbication");

            migrationBuilder.AddForeignKey(
                name: "FK_RiceLots_Ubications_IdLastUbication",
                table: "RiceLots",
                column: "IdLastUbication",
                principalTable: "Ubications",
                principalColumn: "IdUbication");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiceLots_Ubications_IdLastUbication",
                table: "RiceLots");

            migrationBuilder.DropIndex(
                name: "IX_RiceLots_IdLastUbication",
                table: "RiceLots");


            migrationBuilder.CreateIndex(
                name: "IX_RiceLots_IdLastUbication",
                table: "RiceLots",
                column: "IdLastUbication",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RiceLots_Ubications_IdLastUbication",
                table: "RiceLots",
                column: "IdLastUbication",
                principalTable: "Ubications",
                principalColumn: "IdUbication",
                onDelete: ReferentialAction.SetNull);

        }
    }
}
