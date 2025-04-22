using System;

namespace Backend.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    // relation with trip
        public ICollection<Trip> Trips { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        
}
