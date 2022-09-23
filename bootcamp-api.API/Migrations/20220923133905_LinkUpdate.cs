using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bootcamp_api.Migrations
{
    public partial class LinkUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Petfinder_link",
                table: "Bookmarks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Petfinder_link",
                table: "Bookmarks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
