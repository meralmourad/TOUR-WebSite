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
        public DbSet<Category> Categories { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Tourist> Tourists { get; set; }
        public DbSet<TravelAgency> TravelAgencies { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
                /*relationships*/ 
        
        // Bookings
            modelBuilder.Entity<Booking>()
        .HasKey(b => new { b.TouristId, b.TripId });

    modelBuilder.Entity<Booking>()
        .HasOne(b => b.Tourist)
        .WithMany(t => t.Bookings)
        .HasForeignKey(b => b.TouristId)
        .OnDelete(DeleteBehavior.Restrict); // Prevent deleting tourist if there are bookings

    modelBuilder.Entity<Booking>()
        .HasOne(b => b.Trip)
        .WithMany(t => t.Bookings)
        .HasForeignKey(b => b.TripId)
        .OnDelete(DeleteBehavior.Restrict); // Prevent deleting trip if there are bookings

    // ----- Messages -----
    modelBuilder.Entity<Message>()
        .HasOne(m => m.Receiver)
        .WithMany(u => u.ReceivedMessages)
        .HasForeignKey(m => m.ReceiverId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Message>()
        .HasOne(m => m.Sender)
        .WithMany(u => u.SentMessages)
        .HasForeignKey(m => m.SenderId)
        .OnDelete(DeleteBehavior.Restrict);

    // ----- Notifications -----
    modelBuilder.Entity<Notification>()
        .HasOne(n => n.Receiver)
        .WithMany(u => u.ReceivedNotifications)
        .HasForeignKey(n => n.ReceiverId)
        .OnDelete(DeleteBehavior.Restrict);

    modelBuilder.Entity<Notification>()
        .HasOne(n => n.Sender)
        .WithMany(u => u.SentNotifications)
        .HasForeignKey(n => n.SenderId)
        .OnDelete(DeleteBehavior.Restrict);

    // ----- Reports -----
    modelBuilder.Entity<Report>()
        .HasKey(r => new { r.TripId, r.SenderId });

    modelBuilder.Entity<Report>()
        .HasOne(r => r.Trip)
        .WithMany(t => t.Reports)
        .HasForeignKey(r => r.TripId)
        .OnDelete(DeleteBehavior.Cascade); // If trip is deleted, delete reports

    modelBuilder.Entity<Report>()
        .HasOne(r => r.Sender)
        .WithMany(u => u.Reports)
        .HasForeignKey(r => r.SenderId)
        .OnDelete(DeleteBehavior.Restrict); // Prevent deleting user if there are reports 
    }
}
