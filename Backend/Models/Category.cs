using System;

namespace Backend.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public ICollection<Trip> Trips { get; set; } = null!;
    public ICollection<Booking> Bookings { get; set; }
    public ICollection<TripCategory> TripCategories { get; set; } = null!;
}
