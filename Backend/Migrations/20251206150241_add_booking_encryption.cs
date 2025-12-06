using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class add_booking_encryption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reports",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TripCategories",
                keyColumns: new[] { "categoryId", "tripId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "TripCategories",
                keyColumns: new[] { "categoryId", "tripId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "TripCategories",
                keyColumns: new[] { "categoryId", "tripId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "TripCategories",
                keyColumns: new[] { "categoryId", "tripId" },
                keyValues: new object[] { 6, 2 });

            migrationBuilder.DeleteData(
                table: "TripCategories",
                keyColumns: new[] { "categoryId", "tripId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "TripCategories",
                keyColumns: new[] { "categoryId", "tripId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "TripCategories",
                keyColumns: new[] { "categoryId", "tripId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "TripCategories",
                keyColumns: new[] { "categoryId", "tripId" },
                keyValues: new object[] { 8, 4 });

            migrationBuilder.DeleteData(
                table: "TripPlaces",
                keyColumns: new[] { "PlaceId", "TripsId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "TripPlaces",
                keyColumns: new[] { "PlaceId", "TripsId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "TripPlaces",
                keyColumns: new[] { "PlaceId", "TripsId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "TripPlaces",
                keyColumns: new[] { "PlaceId", "TripsId" },
                keyValues: new object[] { 5, 4 });

            migrationBuilder.DeleteData(
                table: "UserNotifications",
                keyColumns: new[] { "NotificationId", "ReceiverId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "UserNotifications",
                keyColumns: new[] { "NotificationId", "ReceiverId" },
                keyValues: new object[] { 2, 11 });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Places",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "Password" },
                values: new object[] { "admin@admin.com", "smtKBr3mpfyS3yyQ4JknDg==.Pq1TmWes1vw7N479T1LLFRj9xbUIYkNu0BppQ6TcVR4=" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                table: "Notifications",
                columns: new[] { "Id", "Content", "SenderId", "Title" },
                values: new object[] { 1, "Your trip to Paris has been approved.", 1, "Trip Approved" });

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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "Password" },
                values: new object[] { "admin@example.com", "admin123" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Discriminator", "Email", "IsApproved", "Name", "Password", "PhoneNumber", "Role" },
                values: new object[,]
                {
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
                values: new object[] { 2, "Your booking for Maldives is confirmed.", 3, "Booking Confirmed" });

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
                table: "UserNotifications",
                columns: new[] { "NotificationId", "ReceiverId", "IsRead" },
                values: new object[] { 1, 2, false });

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
                values: new object[] { 2, 11, true });
        }
    }
}
