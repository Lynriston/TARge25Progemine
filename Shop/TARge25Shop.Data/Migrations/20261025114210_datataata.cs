using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TARge25Shop.Data.Migrations
{
    /// <inheritdoc />
    public partial class datataata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RealEstate",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "RealEstate");
        }
    }
}
