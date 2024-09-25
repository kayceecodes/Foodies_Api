using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace foodies_api.Migrations
{
    /// <inheritdoc />
    public partial class ExternalIdAndIntIdBusiness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserLikeBusinessId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "userLikeBusinesses",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "BusinessId",
                table: "userLikeBusinesses",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "Businesses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExternalId = table.Column<string>(type: "text", nullable: true),
                    Alias = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    URL = table.Column<string>(type: "text", nullable: true),
                    ReviewCount = table.Column<int>(type: "integer", nullable: false),
                    Categories = table.Column<List<string>>(type: "text[]", nullable: true),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    StreetAddress = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true),
                    ZipCode = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<string>(type: "text", nullable: true),
                    UserLikeBusinessId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Businesses_userLikeBusinesses_UserLikeBusinessId",
                        column: x => x.UserLikeBusinessId,
                        principalTable: "userLikeBusinesses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BusinessUser",
                columns: table => new
                {
                    BusinessesId = table.Column<int>(type: "integer", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessUser", x => new { x.BusinessesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_BusinessUser_Businesses_BusinessesId",
                        column: x => x.BusinessesId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserLikeBusinessId",
                table: "Users",
                column: "UserLikeBusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_UserLikeBusinessId",
                table: "Businesses",
                column: "UserLikeBusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUser_UsersId",
                table: "BusinessUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_userLikeBusinesses_UserLikeBusinessId",
                table: "Users",
                column: "UserLikeBusinessId",
                principalTable: "userLikeBusinesses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_userLikeBusinesses_UserLikeBusinessId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "BusinessUser");

            migrationBuilder.DropTable(
                name: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserLikeBusinessId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserLikeBusinessId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "userLikeBusinesses",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessId",
                table: "userLikeBusinesses",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
