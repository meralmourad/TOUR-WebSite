using System;
namespace Backend.Models;
public class Trip
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    
            /* The travel agency that created the trip */
    public required int VendorId { get; set; }
    public TravelAgency Vendor { get; set; }
            /*-------*/
            /* category of the trip */
    public List<Category> Categories { get; set; }
    public required List<int> CategoryIds { get; set; }
            /*-------*/
    public required int Price { get; set; } = 0;
    public DateTime StartDate { get; set; }
    public  List<DateTime>? Duration { get; set; } = new List<DateTime>();
    public string Description { get; set; } = string.Empty;
            /* list of Destination */
    public List<Place> Locations { get; set; }
    public required List<int> LocationIds { get; set; }
            /*-------*/
    public string[] Images { get; set; } = Array.Empty<string>();
    public double Rating { get; set; } = 0;
    public int Status { get; set; } = 0; // 0: pending, 1: approved, 2: rejected 3: canceled 4:finished 5: on going 6:hidden
    public int AvailableSets { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // relation with booking
        public List<Booking> Bookings { get; set; }
        public List<Report> Reports { get; set; }
}
/*

*/