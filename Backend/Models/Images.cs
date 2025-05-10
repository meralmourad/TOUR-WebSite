using System;

namespace Backend.Models;

public class Images
{
    public int Id { get; set; }
    public required string ImageUrl { get; set; }
    public int tripId { get; set; }
    public Trip? trip { get; set; } 
}
