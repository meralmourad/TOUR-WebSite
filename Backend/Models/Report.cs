using System;

namespace Backend.Models;

public class Report
{
    public int Id { get; set; }

    public Trip Trip { get; set; }
    public int TripId { get; set; } 
    
    public User Sender { get; set; }
    public int SenderId { get; set; }

    public TravelAgency Agency { get; set; }
    public int AgencyId { get; set; }

    public string Content { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;

}
