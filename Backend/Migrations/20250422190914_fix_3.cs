using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class fix_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Trips_TripId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Places_Trips_TripId",
                table: "Places");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Users_VendorId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Places_TripId",
                table: "Places");

            migrationBuilder.DropIndex(
                name: "IX_Categories_TripId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "CategoryIds",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "LocationIds",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TripCategories",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    TripsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripCategories", x => new { x.CategoriesId, x.TripsId });
                    table.ForeignKey(
                        name: "FK_TripCategories_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripCategories_Trips_TripsId",
                        column: x => x.TripsId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripPlaces",
                columns: table => new
                {
                    LocationsId = table.Column<int>(type: "int", nullable: false),
                    TripsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripPlaces", x => new { x.LocationsId, x.TripsId });
                    table.ForeignKey(
                        name: "FK_TripPlaces_Places_LocationsId",
                        column: x => x.LocationsId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripPlaces_Trips_TripsId",
                        column: x => x.TripsId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_PlaceId",
                table: "Reports",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CategoryId",
                table: "Bookings",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PlaceId",
                table: "Bookings",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_TripCategories_TripsId",
                table: "TripCategories",
                column: "TripsId");

            migrationBuilder.CreateIndex(
                name: "IX_TripPlaces_TripsId",
                table: "TripPlaces",
                column: "TripsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Categories_CategoryId",
                table: "Bookings",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Places_PlaceId",
                table: "Bookings",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Places_PlaceId",
                table: "Reports",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Users_VendorId",
                table: "Trips",
                column: "VendorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Categories_CategoryId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Places_PlaceId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Places_PlaceId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Users_VendorId",
                table: "Trips");

            migrationBuilder.DropTable(
                name: "TripCategories");

            migrationBuilder.DropTable(
                name: "TripPlaces");

            migrationBuilder.DropIndex(
                name: "IX_Reports_PlaceId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_CategoryId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_PlaceId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CategoryIds",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "LocationIds",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "TripId",
                table: "Places",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TripId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Places_TripId",
                table: "Places",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_TripId",
                table: "Categories",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Trips_TripId",
                table: "Categories",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_Trips_TripId",
                table: "Places",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Users_VendorId",
                table: "Trips",
                column: "VendorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
