using System;

namespace Backend.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    // relation with trip
    public ICollection<Trip> Trips { get; set; } = null!; // Add navigation property for Trips
    public ICollection<Booking> Bookings { get; set; }
    public ICollection<TripCategory> TripCategories { get; set; } = null!;
}
