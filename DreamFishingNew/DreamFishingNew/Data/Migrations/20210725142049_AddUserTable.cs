using Microsoft.EntityFrameworkCore.Migrations;

namespace DreamFishingNew.Data.Migrations
{
    public partial class AddUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ProductCarts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductCartId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProductCartId",
                table: "AspNetUsers",
                column: "ProductCartId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ProductCarts_ProductCartId",
                table: "AspNetUsers",
                column: "ProductCartId",
                principalTable: "ProductCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ProductCarts_ProductCartId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProductCartId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProductCarts");

            migrationBuilder.DropColumn(
                name: "ProductCartId",
                table: "AspNetUsers");
        }
    }
}
