using System;
using Backend.Models;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Diagnostics;

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
        
        // Configure transparent encryption for sensitive columns
        var encryptConverter = new ValueConverter<string, string>(
            v => v == null ? null : AesEncryption.Encrypt(v),
            v => v == null ? null : AesEncryption.Decrypt(v));

        modelBuilder.Entity<Booking>()
            .Property(b => b.PhoneNumber)
            .HasConversion(encryptConverter);

        modelBuilder.Entity<Booking>()
            .Property(b => b.Comment)
            .HasConversion(encryptConverter);
        
        // Also encrypt user phone numbers
        modelBuilder.Entity<User>()
            .Property(u => u.PhoneNumber)
            .HasConversion(encryptConverter);
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

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Admin", Email = "admin@admin.com", Password = "smtKBr3mpfyS3yyQ4JknDg==.Pq1TmWes1vw7N479T1LLFRj9xbUIYkNu0BppQ6TcVR4=", Role = "Admin", PhoneNumber = "lfib0cCCAAwnKNmt/lo3qtcvG1g5uMc644CxSjaA680OpesFeS4h", Address = "Admin Address" ,IsApproved= true }
        );
  }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
        base.OnConfiguring(optionsBuilder);
    }
}
