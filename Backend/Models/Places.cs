using System;

namespace Backend.Models
{
    public class Place
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string[] ImagesURL { get; set; } = Array.Empty<string>();
        // relation with trip
        public ICollection<Trip> Trips { get; set; }
        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Report> Reports { get; set; }
        
    }
}
