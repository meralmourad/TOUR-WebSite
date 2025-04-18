using System;

namespace Backend.Models;
public class TravelAgency : User
{
    public ICollection<Trip>? Trips { get; set; }
}
