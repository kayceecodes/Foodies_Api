using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace foodies_api.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropertiesToBusiness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "Businesses",
                newName: "Zipcode");

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Businesses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Businesses",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Businesses");

            migrationBuilder.RenameColumn(
                name: "Zipcode",
                table: "Businesses",
                newName: "ZipCode");
        }
    }
}
