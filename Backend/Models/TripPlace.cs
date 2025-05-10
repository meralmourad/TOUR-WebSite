using System;

namespace Backend.Models;

public class TripPlace
{
        public int TripsId { get; set; }
        public required Trip Trip { get; set; }
        public int PlaceId { get; set; }
        public required Place Place { get; set; }
        
}
