using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class fin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AvailableSets = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_Users_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserNotifications",
                columns: table => new
                {
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotifications", x => new { x.ReceiverId, x.NotificationId });
                    table.ForeignKey(
                        name: "FK_UserNotifications_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserNotifications_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TouristId = table.Column<int>(type: "int", nullable: false),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    TravelAgencyId = table.Column<int>(type: "int", nullable: false),
                    SeatsNumber = table.Column<int>(type: "int", nullable: false),
                    IsApproved = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    PlaceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bookings_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bookings_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_TouristId",
                        column: x => x.TouristId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_TravelAgencyId",
                        column: x => x.TravelAgencyId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CategoryTrip",
                columns: table => new
                {
                    CategoriesId = table.Column<int>(type: "int", nullable: false),
                    TripsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTrip", x => new { x.CategoriesId, x.TripsId });
                    table.ForeignKey(
                        name: "FK_CategoryTrip_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryTrip_Trips_TripsId",
                        column: x => x.TripsId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tripId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Trips_tripId",
                        column: x => x.tripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    AgencyId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TripCategories",
                columns: table => new
                {
                    tripId = table.Column<int>(type: "int", nullable: false),
                    categoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripCategories", x => new { x.tripId, x.categoryId });
                    table.ForeignKey(
                        name: "FK_TripCategories_Categories_categoryId",
                        column: x => x.categoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TripCategories_Trips_tripId",
                        column: x => x.tripId,
                        principalTable: "Trips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TripPlaces",
                columns: table => new
                {
                    TripsId = table.Column<int>(type: "int", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripPlaces", x => new { x.TripsId, x.PlaceId });
                    table.ForeignKey(
                        name: "FK_TripPlaces_Places_PlaceId",
                        column: x => x.PlaceId,
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

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Adventure" },
                    { 2, "Relaxation" },
                    { 3, "Cultural" },
                    { 4, "Nature" },
                    { 5, "Historical" },
                    { 6, "Luxury" },
                    { 7, "Family" },
                    { 8, "Romantic" },
                    { 9, "Wildlife" },
                    { 10, "Sports" },
                    { 11, "Beach" },
                    { 12, "Adventure Sports" }
                });

            migrationBuilder.InsertData(
                table: "Places",
                columns: new[] { "Id", "Country", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "France", "The city of lights.", "Paris" },
                    { 2, "Maldives", "Tropical paradise.", "Maldives" },
                    { 3, "Italy", "The Eternal City.", "Rome" },
                    { 4, "USA", "The city that never sleeps.", "New York" },
                    { 5, "Japan", "A blend of tradition and modernity.", "Tokyo" },
                    { 6, "Australia", "The Harbour City.", "Sydney" },
                    { 7, "South Africa", "A city of stunning landscapes.", "Cape Town" },
                    { 8, "Brazil", "The Marvelous City.", "Rio de Janeiro" },
                    { 9, "UAE", "The city of gold.", "Dubai" },
                    { 10, "Turkey", "Where East meets West.", "Istanbul" },
                    { 11, "Greece", "A picturesque island in the Aegean Sea.", "Santorini" },
                    { 12, "Indonesia", "A tropical paradise with stunning beaches.", "Bali" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Discriminator", "Email", "IsApproved", "Name", "Password", "PhoneNumber", "Role" },
                values: new object[,]
                {
                    { 1, "Admin Address", "User", "admin@example.com", true, "Admin", "admin123", "1234567890", "Admin" },
                    { 2, "123 Adventure Lane", "User", "agency1@example.com", true, "Global Adventures", "agency123", "5551234567", "Agency" },
                    { 3, "Adventure Lane", "User", "agency2@example.com", true, "Adventure Co.", "password", "1111111111", "Agency" },
                    { 4, "Luxury Street", "User", "agency3@example.com", true, "Luxury Travels", "password", "2222222222", "Agency" },
                    { 5, "Alice's Address", "User", "alice@example.com", true, "Alice", "password", "1231231234", "Tourist" },
                    { 6, "Bob's Address", "User", "bob@example.com", true, "Bob", "password", "2342342345", "Tourist" },
                    { 7, "Charlie's Address", "User", "charlie@example.com", true, "Charlie", "password", "3453453456", "Tourist" },
                    { 8, "David's Address", "User", "david@example.com", true, "David", "password", "4564564567", "Tourist" },
                    { 9, "Eve's Address", "User", "eve@example.com", true, "Eve", "password", "5675675678", "Tourist" },
                    { 10, "Frank's Address", "User", "frank@example.com", true, "Frank", "password", "6786786789", "Tourist" },
                    { 11, "Grace's Address", "User", "grace@example.com", true, "Grace", "password", "7897897890", "Tourist" },
                    { 12, "Hank's Address", "User", "hank@example.com", true, "Hank", "password", "8908908901", "Tourist" },
                    { 13, "Ivy's Address", "User", "ivy@example.com", true, "Ivy", "password", "9019019012", "Tourist" },
                    { 14, "Jack's Address", "User", "jack@example.com", true, "Jack", "password", "1234561234", "Tourist" },
                    { 15, "Karen's Address", "User", "karen@example.com", true, "Karen", "password", "2345672345", "Tourist" },
                    { 16, "Leo's Address", "User", "leo@example.com", true, "Leo", "password", "3456783456", "Tourist" },
                    { 17, "Mona's Address", "User", "mona@example.com", true, "Mona", "password", "4567894567", "Tourist" },
                    { 18, "Nina's Address", "User", "nina@example.com", true, "Nina", "password", "5678905678", "Tourist" }
                });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "Id", "Content", "ReceiverId", "SenderId" },
                values: new object[,]
                {
                    { 1, "Welcome to the platform!", 2, 1 },
                    { 2, "Thank you!", 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "Content", "SenderId", "Title" },
                values: new object[,]
                {
                    { 1, "Your trip to Paris has been approved.", 1, "Trip Approved" },
                    { 2, "Your booking for Maldives is confirmed.", 3, "Booking Confirmed" }
                });

            migrationBuilder.InsertData(
                table: "Trips",
                columns: new[] { "Id", "AvailableSets", "Description", "EndDate", "Price", "Rating", "StartDate", "Status", "Title", "VendorId" },
                values: new object[,]
                {
                    { 1, 20, "Explore the beauty of Paris with this amazing adventure package.", new DateOnly(2023, 6, 10), 1500, 4.5, new DateOnly(2023, 6, 1), 1, "Paris Adventure", 3 },
                    { 2, 15, "Relax and unwind in the tropical paradise of Maldives.", new DateOnly(2023, 7, 8), 2000, 4.7999999999999998, new DateOnly(2023, 7, 1), 1, "Maldives Getaway", 3 },
                    { 3, 25, "Discover the wonders of Rome.", new DateOnly(2023, 8, 10), 1200, 4.7000000000000002, new DateOnly(2023, 8, 1), 1, "Rome Discovery", 4 },
                    { 4, 30, "Experience the culture of Tokyo.", new DateOnly(2023, 9, 12), 1800, 4.9000000000000004, new DateOnly(2023, 9, 1), 1, "Tokyo Experience", 4 },
                    { 5, 20, "Explore the beauty of Sydney.", new DateOnly(2023, 10, 10), 1700, 4.5999999999999996, new DateOnly(2023, 10, 1), 1, "Sydney Adventure", 3 },
                    { 6, 15, "Experience the wildlife of Cape Town.", new DateOnly(2023, 11, 12), 2500, 4.9000000000000004, new DateOnly(2023, 11, 1), 1, "Cape Town Safari", 4 }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "CategoryId", "Comment", "IsApproved", "PhoneNumber", "PlaceId", "Rating", "SeatsNumber", "TouristId", "TravelAgencyId", "TripId" },
                values: new object[,]
                {
                    { 1, null, null, 1, null, null, -1, 2, 2, 3, 1 },
                    { 2, null, null, 0, null, null, -1, 1, 11, 3, 2 },
                    { 3, null, null, -1, null, null, -1, 3, 12, 4, 3 },
                    { 4, null, null, 1, null, null, -1, 4, 13, 4, 4 },
                    { 5, null, null, 1, null, null, -1, 2, 14, 3, 5 },
                    { 6, null, null, 0, null, null, -1, 1, 15, 4, 6 }
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "Id", "ImageUrl", "tripId" },
                values: new object[,]
                {
                    { 1, "https://example.com/paris1.jpg", 1 },
                    { 2, "https://example.com/paris2.jpg", 1 },
                    { 3, "https://example.com/maldives1.jpg", 2 },
                    { 4, "https://example.com/rome1.jpg", 3 },
                    { 5, "https://example.com/sydney1.jpg", 5 },
                    { 6, "https://example.com/capetown1.jpg", 6 }
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "Id", "AgencyId", "Content", "IsRead", "PlaceId", "SenderId", "TripId" },
                values: new object[,]
                {
                    { 1, 3, "Great trip!", false, null, 2, 1 },
                    { 2, 3, "Had some issues.", true, null, 11, 2 }
                });

            migrationBuilder.InsertData(
                table: "TripCategories",
                columns: new[] { "categoryId", "tripId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 3, 1 },
                    { 2, 2 },
                    { 6, 2 },
                    { 3, 3 },
                    { 5, 3 },
                    { 4, 4 },
                    { 8, 4 }
                });

            migrationBuilder.InsertData(
                table: "TripPlaces",
                columns: new[] { "PlaceId", "TripsId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 },
                    { 5, 4 }
                });

            migrationBuilder.InsertData(
                table: "UserNotifications",
                columns: new[] { "NotificationId", "ReceiverId", "IsRead" },
                values: new object[,]
                {
                    { 1, 2, false },
                    { 2, 11, true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CategoryId",
                table: "Bookings",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PlaceId",
                table: "Bookings",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TouristId",
                table: "Bookings",
                column: "TouristId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TravelAgencyId",
                table: "Bookings",
                column: "TravelAgencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_TripId",
                table: "Bookings",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTrip_TripsId",
                table: "CategoryTrip",
                column: "TripsId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_tripId",
                table: "Images",
                column: "tripId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SenderId",
                table: "Notifications",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_PlaceId",
                table: "Reports",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SenderId",
                table: "Reports",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_TripId",
                table: "Reports",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_TripCategories_categoryId",
                table: "TripCategories",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TripPlaces_PlaceId",
                table: "TripPlaces",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_VendorId",
                table: "Trips",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_NotificationId",
                table: "UserNotifications",
                column: "NotificationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "CategoryTrip");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "TripCategories");

            migrationBuilder.DropTable(
                name: "TripPlaces");

            migrationBuilder.DropTable(
                name: "UserNotifications");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropTable(
                name: "Trips");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
