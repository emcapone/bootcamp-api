using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bootcamp_api.Migrations
{
    public partial class UserUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Petfinder_Id",
                table: "Bookmarks",
                newName: "Petfinder_id");

            migrationBuilder.RenameColumn(
                name: "External_Url",
                table: "Bookmarks",
                newName: "External_url");

            migrationBuilder.RenameColumn(
                name: "Link",
                table: "Bookmarks",
                newName: "Petfinder_link");

            migrationBuilder.AddColumn<int>(
                name: "User_id",
                table: "Pets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "User_id",
                table: "Bookmarks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pets_User_id",
                table: "Pets",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_User_id",
                table: "Bookmarks",
                column: "User_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookmarks_Users_User_id",
                table: "Bookmarks",
                column: "User_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Users_User_id",
                table: "Pets",
                column: "User_id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookmarks_Users_User_id",
                table: "Bookmarks");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Users_User_id",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Pets_User_id",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Bookmarks_User_id",
                table: "Bookmarks");

            migrationBuilder.DropColumn(
                name: "User_id",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "User_id",
                table: "Bookmarks");

            migrationBuilder.RenameColumn(
                name: "Petfinder_id",
                table: "Bookmarks",
                newName: "Petfinder_Id");

            migrationBuilder.RenameColumn(
                name: "External_url",
                table: "Bookmarks",
                newName: "External_Url");

            migrationBuilder.RenameColumn(
                name: "Petfinder_link",
                table: "Bookmarks",
                newName: "Link");
        }
    }
}
