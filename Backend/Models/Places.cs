using System;

namespace Backend.Models
{
    public class Place
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<TripPlace>? Trip_Places { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Report>? Reports { get; set; }
        
    }
}
