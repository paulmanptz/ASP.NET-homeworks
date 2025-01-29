using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromoCodeFactory.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class NewCode2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPreference_Customers_CustomerId",
                table: "CustomerPreference");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPreference_Preferences_PreferenceId",
                table: "CustomerPreference");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerPreference",
                table: "CustomerPreference");

            migrationBuilder.RenameTable(
                name: "CustomerPreference",
                newName: "CustomerPreferences");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerPreference_PreferenceId",
                table: "CustomerPreferences",
                newName: "IX_CustomerPreferences_PreferenceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerPreferences",
                table: "CustomerPreferences",
                columns: new[] { "CustomerId", "PreferenceId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPreferences_Customers_CustomerId",
                table: "CustomerPreferences",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPreferences_Preferences_PreferenceId",
                table: "CustomerPreferences",
                column: "PreferenceId",
                principalTable: "Preferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPreferences_Customers_CustomerId",
                table: "CustomerPreferences");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerPreferences_Preferences_PreferenceId",
                table: "CustomerPreferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerPreferences",
                table: "CustomerPreferences");

            migrationBuilder.RenameTable(
                name: "CustomerPreferences",
                newName: "CustomerPreference");

            migrationBuilder.RenameIndex(
                name: "IX_CustomerPreferences_PreferenceId",
                table: "CustomerPreference",
                newName: "IX_CustomerPreference_PreferenceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerPreference",
                table: "CustomerPreference",
                columns: new[] { "CustomerId", "PreferenceId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPreference_Customers_CustomerId",
                table: "CustomerPreference",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerPreference_Preferences_PreferenceId",
                table: "CustomerPreference",
                column: "PreferenceId",
                principalTable: "Preferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
