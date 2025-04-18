using System;

namespace Backend.Models;

public class Tourist:User
{
        public ICollection<Booking>? Bookings { get; set; }
}
