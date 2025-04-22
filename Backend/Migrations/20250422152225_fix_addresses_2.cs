using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class fix_addresses_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Users_VendorId1",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_VendorId1",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "VendorId1",
                table: "Trips");

            migrationBuilder.AlterColumn<int>(
                name: "VendorId",
                table: "Trips",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_VendorId",
                table: "Trips",
                column: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Users_VendorId",
                table: "Trips",
                column: "VendorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Users_VendorId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_VendorId",
                table: "Trips");

            migrationBuilder.AlterColumn<string>(
                name: "VendorId",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "VendorId1",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Trips_VendorId1",
                table: "Trips",
                column: "VendorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Users_VendorId1",
                table: "Trips",
                column: "VendorId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
