using System;

namespace Backend.Models;

public class Booking
{
    public int Id { get; set; } // Add primary key

    /* Tourist relation */
    public Tourist Tourist { get; set; }
    public int TouristId { get; set; }            
            /* Trip relation*/
    public Trip Trip { get; set; } 
    public int TripId { get; set; }    
    /* TravelAgency relation */
    public TravelAgency TravelAgency { get; set; }
    public int TravelAgencyId { get; set; } 
    public int SeatsNumber { get; set; } = 1;
    public int IsApproved { get; set; } = 0; //-1:reject 0: pending, 1: approved 
    public String? PhoneNumber { get; set; }
    public String? Comment { get; set; }
    
    // -1: not rated, 0: bad, 1: good, 2: very good, 3: excellent
    public int Rating { get; set; } = -1;


}
