using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mystore.Migrations
{
    public partial class AddEcommerceModels3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_AspNetUsers_CategoryId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CategoryId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "CartItems");

          migrationBuilder.DropColumn(
          name: "Contacts",
          table: "AspNetUsers");
        
    }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryId",
                table: "CartItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CategoryId",
                table: "CartItems",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_AspNetUsers_CategoryId",
                table: "CartItems",
                column: "CategoryId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
