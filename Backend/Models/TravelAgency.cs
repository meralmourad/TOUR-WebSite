using System;

namespace Backend.Models;
public class TravelAgency : User
{
    public ICollection<Trip>? Trips { get; set; }
    public ICollection<Booking>? Bookings { get; set; }
    public ICollection<Report>? Reports { get; set; }
    
}
