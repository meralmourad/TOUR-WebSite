namespace Backend.DTOs;

public class TripDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Price { get; set; }
    public DateTime StartDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public double Rating { get; set; }
}
