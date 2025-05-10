using System;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Backend.Models.Category> Categories { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Tourist> Tourists { get; set; }
    public DbSet<TravelAgency> TravelAgencies { get; set; }
    public DbSet<Trip> Trips { get; set; }
    public DbSet<User> Users { get; set; }

    public DbSet<Images> Images { get; set; }
    public DbSet<TripCategory> TripCategories { get; set; }
    public DbSet<TripPlace> TripPlaces { get; set; }
    public DbSet<UserNotification> UserNotifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        /*relationships*/

        modelBuilder.Entity<TripCategory>()
            .HasKey(tc => new { tc.tripId, tc.categoryId });

        modelBuilder.Entity<TripCategory>()
            .HasOne(tc => tc.Trip)
            .WithMany(t => t.TripCategories)
            .HasForeignKey(tc => tc.tripId);

        modelBuilder.Entity<TripCategory>()
            .HasOne(tc => tc.Category)
            .WithMany(c => c.TripCategories)
            .HasForeignKey(tc => tc.categoryId);

        modelBuilder.Entity<TripPlace>()
            .HasKey(tp => new { tp.TripsId, tp.PlaceId });

        modelBuilder.Entity<TripPlace>()
            .HasOne(tp => tp.Trip)
            .WithMany(t => t.TripPlaces)
            .HasForeignKey(tp => tp.TripsId);

        modelBuilder.Entity<TripPlace>()
            .HasOne(tp => tp.Place)
            .WithMany(p => p.Trip_Places)
            .HasForeignKey(tp => tp.PlaceId);

        modelBuilder.Entity<Images>()
            .HasOne(i => i.trip)
            .WithMany(t => t.Image)
            .HasForeignKey(i => i.tripId);

        // User-messages relationships
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.SentMessages)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Receiver)
            .WithMany(u => u.ReceivedMessages)
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.Sender)
            .WithMany(u => u.SentNotifications)
            .HasForeignKey(n => n.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserNotification>()
            .HasKey(un => new {  un.ReceiverId , un.NotificationId });

        modelBuilder.Entity<UserNotification>()
            .HasOne(un => un.Receiver)
            .WithMany(u => u.UserNotifications)
            .HasForeignKey(un => un.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserNotification>()
            .HasOne(un => un.Notification)
            .WithMany(n => n.UserNotifications)
            .HasForeignKey(un => un.NotificationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Trip>()
            .HasOne(t => t.Vendor)
            .WithMany(a => a.Trips)
            .HasForeignKey(t => t.VendorId);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Trip)
            .WithMany(t => t.Bookings)
            .HasForeignKey(b => b.TripId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Tourist)
            .WithMany(t => t.Bookings)
            .HasForeignKey(b => b.TouristId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.TravelAgency)
            .WithMany(a => a.Bookings)
            .HasForeignKey(b => b.TravelAgencyId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Report>()
            .HasOne(r => r.Trip)
            .WithMany(t => t.Reports)
            .HasForeignKey(r => r.TripId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Report>()
            .HasOne(r => r.Sender)
            .WithMany(u => u.Reports)
            .HasForeignKey(r => r.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Report>()
            .Ignore(r => r.Agency);

        // Seed default data
        modelBuilder.Entity<Backend.Models.Category>().HasData(
            new Backend.Models.Category { Id = 1, Name = "Adventure"},
            new Backend.Models.Category { Id = 2, Name = "Relaxation" },
            new Backend.Models.Category { Id = 3, Name = "Cultural" },
            new Backend.Models.Category { Id = 4, Name = "Nature" },
            new Backend.Models.Category { Id = 5, Name = "Historical" },
            new Backend.Models.Category { Id = 6, Name = "Luxury"},
            new Backend.Models.Category { Id = 7, Name = "Family" },
            new Backend.Models.Category { Id = 8, Name = "Romantic" },
            new Backend.Models.Category { Id = 9, Name = "Wildlife" },
            new Backend.Models.Category { Id = 10, Name = "Sports", },
            new Backend.Models.Category { Id = 11, Name = "Beach" },
            new Backend.Models.Category { Id = 12, Name = "Adventure Sports" }
        );

        modelBuilder.Entity<Place>().HasData(
            new Place { Id = 1, Name = "Paris", Country = "France", Description = "The city of lights." },
            new Place { Id = 2, Name = "Maldives", Country = "Maldives", Description = "Tropical paradise." },
            new Place { Id = 3, Name = "Rome", Country = "Italy", Description = "The Eternal City." },
            new Place { Id = 4, Name = "New York", Country = "USA", Description = "The city that never sleeps." },
            new Place { Id = 5, Name = "Tokyo", Country = "Japan", Description = "A blend of tradition and modernity." },
            new Place { Id = 6, Name = "Sydney", Country = "Australia", Description = "The Harbour City." },
            new Place { Id = 7, Name = "Cape Town", Country = "South Africa", Description = "A city of stunning landscapes." },
            new Place { Id = 8, Name = "Rio de Janeiro", Country = "Brazil", Description = "The Marvelous City." },
            new Place { Id = 9, Name = "Dubai", Country = "UAE", Description = "The city of gold." },
            new Place { Id = 10, Name = "Istanbul", Country = "Turkey", Description = "Where East meets West." },
            new Place { Id = 11, Name = "Santorini", Country = "Greece", Description = "A picturesque island in the Aegean Sea." },
            new Place { Id = 12, Name = "Bali", Country = "Indonesia", Description = "A tropical paradise with stunning beaches." }
        );

        // Seed default data for User
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Admin", Email = "admin@example.com", Password = "admin123", Role = "Admin", PhoneNumber = "1234567890", Address = "Admin Address" },
            new User { Id = 2, Name = "Global Adventures", Email = "agency1@example.com", Password = "agency123", Role = "Agency", PhoneNumber = "5551234567", Address = "123 Adventure Lane" },
            new User { Id = 3, Name = "Adventure Co.", Email = "agency2@example.com", Password = "password", Role = "Agency", PhoneNumber = "1111111111", Address = "Adventure Lane" },
            new User { Id = 4, Name = "Luxury Travels", Email = "agency3@example.com", Password = "password", Role = "Agency", PhoneNumber = "2222222222", Address = "Luxury Street" },
            new User { Id = 5, Name = "Alice", Email = "alice@example.com", Password = "password", Role = "Tourist", PhoneNumber = "1231231234", Address = "Alice's Address" },
            new User { Id = 6, Name = "Bob", Email = "bob@example.com", Password = "password", Role = "Tourist", PhoneNumber = "2342342345", Address = "Bob's Address" },
            new User { Id = 7, Name = "Charlie", Email = "charlie@example.com", Password = "password", Role = "Tourist", PhoneNumber = "3453453456", Address = "Charlie's Address" },
            new User { Id = 8, Name = "David", Email = "david@example.com", Password = "password", Role = "Tourist", PhoneNumber = "4564564567", Address = "David's Address" },
            new User { Id = 9, Name = "Eve", Email = "eve@example.com", Password = "password", Role = "Tourist", PhoneNumber = "5675675678", Address = "Eve's Address" },
            new User { Id = 10, Name = "Frank", Email = "frank@example.com", Password = "password", Role = "Tourist", PhoneNumber = "6786786789", Address = "Frank's Address" },
            new User { Id = 11, Name = "Grace", Email = "grace@example.com", Password = "password", Role = "Tourist", PhoneNumber = "7897897890", Address = "Grace's Address" },
            new User { Id = 12, Name = "Hank", Email = "hank@example.com", Password = "password", Role = "Tourist", PhoneNumber = "8908908901", Address = "Hank's Address" },
            new User { Id = 13, Name = "Ivy", Email = "ivy@example.com", Password = "password", Role = "Tourist", PhoneNumber = "9019019012", Address = "Ivy's Address" },
            new User { Id = 14, Name = "Jack", Email = "jack@example.com", Password = "password", Role = "Tourist", PhoneNumber = "1234561234", Address = "Jack's Address" },
            new User { Id = 15, Name = "Karen", Email = "karen@example.com", Password = "password", Role = "Tourist", PhoneNumber = "2345672345", Address = "Karen's Address" },
            new User { Id = 16, Name = "Leo", Email = "leo@example.com", Password = "password", Role = "Tourist", PhoneNumber = "3456783456", Address = "Leo's Address" },
            new User { Id = 17, Name = "Mona", Email = "mona@example.com", Password = "password", Role = "Tourist", PhoneNumber = "4567894567", Address = "Mona's Address" },
            new User { Id = 18, Name = "Nina", Email = "nina@example.com", Password = "password", Role = "Tourist", PhoneNumber = "5678905678", Address = "Nina's Address" }
        );

        modelBuilder.Entity<Trip>().HasData(
            new Trip
            {
                Id = 1,
                Title = "Paris Adventure",
                VendorId = 3,
                Price = 1500,
                StartDate = DateOnly.FromDateTime(new DateTime(2023, 6, 1)),
                EndDate = DateOnly.FromDateTime(new DateTime(2023, 6, 10)),
                Description = "Explore the beauty of Paris with this amazing adventure package.",
                Rating = 4.5,
                Status = 1,
                AvailableSets = 20
            },
            new Trip
            {
                Id = 2,
                Title = "Maldives Getaway",
                VendorId = 3,
                Price = 2000,
                StartDate = DateOnly.FromDateTime(new DateTime(2023, 7, 1)),
                EndDate = DateOnly.FromDateTime(new DateTime(2023, 7, 8)),
                Description = "Relax and unwind in the tropical paradise of Maldives.",
                Rating = 4.8,
                Status = 1,
                AvailableSets = 15
            },
            new Trip
            {
                Id = 3,
                Title = "Rome Discovery",
                VendorId = 4,
                Price = 1200,
                StartDate = DateOnly.FromDateTime(new DateTime(2023, 8, 1)),
                EndDate = DateOnly.FromDateTime(new DateTime(2023, 8, 10)),
                Description = "Discover the wonders of Rome.",
                Rating = 4.7,
                Status = 1,
                AvailableSets = 25
            },
            new Trip
            {
                Id = 4,
                Title = "Tokyo Experience",
                VendorId = 4,
                Price = 1800,
                StartDate = DateOnly.FromDateTime(new DateTime(2023, 9, 1)),
                EndDate = DateOnly.FromDateTime(new DateTime(2023, 9, 12)),
                Description = "Experience the culture of Tokyo.",
                Rating = 4.9,
                Status = 1,
                AvailableSets = 30
            },
            new Trip
            {
                Id = 5,
                Title = "Sydney Adventure",
                VendorId = 3,
                Price = 1700,
                StartDate = DateOnly.FromDateTime(new DateTime(2023, 10, 1)),
                EndDate = DateOnly.FromDateTime(new DateTime(2023, 10, 10)),
                Description = "Explore the beauty of Sydney.",
                Rating = 4.6,
                Status = 1,
                AvailableSets = 20
            },
            new Trip
            {
                Id = 6,
                Title = "Cape Town Safari",
                VendorId = 4,
                Price = 2500,
                StartDate = DateOnly.FromDateTime(new DateTime(2023, 11, 1)),
                EndDate = DateOnly.FromDateTime(new DateTime(2023, 11, 12)),
                Description = "Experience the wildlife of Cape Town.",
                Rating = 4.9,
                Status = 1,
                AvailableSets = 15
            }
        );

        modelBuilder.Entity<TripCategory>().HasData(
            new TripCategory { tripId = 1, categoryId = 1 },
            new TripCategory { tripId = 1, categoryId = 3 },
            new TripCategory { tripId = 2, categoryId = 2 },
            new TripCategory { tripId = 2, categoryId = 6 },
            new TripCategory { tripId = 3, categoryId = 3 },
            new TripCategory { tripId = 3, categoryId = 5 },
            new TripCategory { tripId = 4, categoryId = 4 },
            new TripCategory { tripId = 4, categoryId = 8 }
        );

        modelBuilder.Entity<TripPlace>().HasData(
            new  { TripsId = 1, PlaceId = 1 },
            new  { TripsId = 2, PlaceId = 2 },
            new  { TripsId = 3, PlaceId = 3 },
            new  { TripsId = 4, PlaceId = 5 }
        );

        modelBuilder.Entity<Booking>().HasData(
            new Booking
            {
                Id = 1,
                TripId = 1,
                TouristId = 2,
                TravelAgencyId = 3,
                IsApproved = 1, // 1: confirmed
                SeatsNumber = 2
            },
            new Booking
            {
                Id = 2,
                TripId = 2,
                TouristId = 11,
                TravelAgencyId = 3,
                IsApproved = 0,
                SeatsNumber = 1
            },
            new Booking
            {
                Id = 3,
                TripId = 3,
                TouristId = 12,
                TravelAgencyId = 4,
                IsApproved = -1,
                SeatsNumber = 3
            },
            new Booking
            {
                Id = 4,
                TripId = 4,
                TouristId = 13,
                TravelAgencyId = 4,
                IsApproved = 1,
                SeatsNumber = 4
            },
            new Booking
            {
                Id = 5,
                TripId = 5,
                TouristId = 14,
                TravelAgencyId = 3,
                IsApproved = 1,
                SeatsNumber = 2
            },
            new Booking
            {
                Id = 6,
                TripId = 6,
                TouristId = 15,
                TravelAgencyId = 4,
                IsApproved = 0,
                SeatsNumber = 1
            }
        );

        // Seed default data for Images
        modelBuilder.Entity<Images>().HasData(
            new Images { Id = 1, tripId = 1, ImageUrl = "https://example.com/paris1.jpg" },
            new Images { Id = 2, tripId = 1, ImageUrl = "https://example.com/paris2.jpg" },
            new Images { Id = 3, tripId = 2, ImageUrl = "https://example.com/maldives1.jpg" },
            new Images { Id = 4, tripId = 3, ImageUrl = "https://example.com/rome1.jpg" },
            new Images { Id = 5, tripId = 5, ImageUrl = "https://example.com/sydney1.jpg" },
            new Images { Id = 6, tripId = 6, ImageUrl = "https://example.com/capetown1.jpg" }
        );

        // Seed default data for Messages
        modelBuilder.Entity<Message>().HasData(
            new Message { Id = 1, Content = "Welcome to the platform!", SenderId = 1, ReceiverId = 2 },
            new Message { Id = 2, Content = "Thank you!", SenderId = 2, ReceiverId = 1 }
        );

        // Seed default data for Notifications
        modelBuilder.Entity<Notification>().HasData(
            new Notification { Id = 1, Title = "Trip Approved", Content = "Your trip to Paris has been approved.", SenderId = 1 },
            new Notification { Id = 2, Title = "Booking Confirmed", Content = "Your booking for Maldives is confirmed.", SenderId = 3 }
        );

        // Seed default data for UserNotifications
        modelBuilder.Entity<UserNotification>().HasData(
            new UserNotification { ReceiverId = 2, NotificationId = 1, IsRead = false },
            new UserNotification { ReceiverId = 11, NotificationId = 2, IsRead = true }
        );

        // Seed default data for Reports
        modelBuilder.Entity<Report>().HasData(
            new Report { Id = 1, TripId = 1, SenderId = 2, AgencyId = 3, Content = "Great trip!", IsRead = false },
            new Report { Id = 2, TripId = 2, SenderId = 11, AgencyId = 3, Content = "Had some issues.", IsRead = true }
        );
    }
}
