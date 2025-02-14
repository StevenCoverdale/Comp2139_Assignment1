using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace assignment1.Migrations
{
    /// <inheritdoc />
    public partial class removedExtra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name2",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name2",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
