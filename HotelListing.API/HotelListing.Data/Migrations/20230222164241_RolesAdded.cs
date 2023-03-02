using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelListing.API.Migrations
{
    /// <inheritdoc />
    public partial class RolesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0eeaa127-b3ee-4828-9384-e67fd6a7a995", "52344861-2d64-41c8-ac10-edf6d85373b3", "User", "USER" },
                    { "5da2fb8b-837e-4abd-82e3-da8741b5f07e", "1b577ee7-1ed0-4dd1-b0e5-8f802fcbdf7a", "Administrator", "ADMINISTRATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0eeaa127-b3ee-4828-9384-e67fd6a7a995");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5da2fb8b-837e-4abd-82e3-da8741b5f07e");
        }
    }
}
