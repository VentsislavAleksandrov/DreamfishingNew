using Microsoft.EntityFrameworkCore.Migrations;

namespace DreamFishingNew.Data.Migrations
{
    public partial class RevertProductCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProductCartId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProductCarts");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProductCartId",
                table: "AspNetUsers",
                column: "ProductCartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProductCartId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ProductCarts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProductCartId",
                table: "AspNetUsers",
                column: "ProductCartId",
                unique: true);
        }
    }
}
