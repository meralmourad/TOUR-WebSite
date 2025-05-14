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
    public required int Price { get; set; } = 0;
    public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
            /*-------*/
    public double Rating { get; set; } = 0;
    public int Status { get; set; } = 0; 
    public int AvailableSets { get; set; } = 0;
    
        public List<Booking>? Bookings { get; set; }
        public List<Report>? Reports { get; set; }
        public List<Images>? Image { get; set; } 
        public List<TripPlace>? TripPlaces { get; set; }
        public List<TripCategory>? TripCategories { get; set; }
        public List<Category>? Categories { get; set; }
}
/*

*/