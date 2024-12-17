using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PromoCodeFactory.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NewCode5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CustomerPreference",
                columns: new[] { "CustomerId", "PreferenceId" },
                values: new object[,]
                {
                    { new Guid("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"), new Guid("c4bda62e-fc74-4256-a956-4760b3858cbd") },
                    { new Guid("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"), new Guid("ef7f299f-92d7-459f-896e-078ed53ef99c") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CustomerPreference",
                keyColumns: new[] { "CustomerId", "PreferenceId" },
                keyValues: new object[] { new Guid("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"), new Guid("c4bda62e-fc74-4256-a956-4760b3858cbd") });

            migrationBuilder.DeleteData(
                table: "CustomerPreference",
                keyColumns: new[] { "CustomerId", "PreferenceId" },
                keyValues: new object[] { new Guid("a6c8c6b1-4349-45b0-ab31-244740aaf0f0"), new Guid("ef7f299f-92d7-459f-896e-078ed53ef99c") });
        }
    }
}
