using System;

namespace Backend.Models;

public class TripCategory
{
    public int tripId { get; set; }
    public Trip? Trip { get; set; } = null!;
    public int categoryId { get; set; }
    public Category? Category { get; set; } = null!;
}
