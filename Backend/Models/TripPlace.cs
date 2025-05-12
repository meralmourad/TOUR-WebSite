using System;

namespace Backend.Models;

public class TripPlace
{
        public int TripsId { get; set; }
        public  Trip Trip { get; set; }
        public int PlaceId { get; set; }
        public  Place Place { get; set; }
        
}
