using System;

namespace Backend.Models;

public class Tourist
{
        public ICollection<Booking>? Bookings { get; set; }

}
