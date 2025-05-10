using Backend.Models;

namespace Backend.DTOs;

public class TripDto
{
    // Properties of the TripDto class
    public int Id { get; set; }
    public  int AgenceId { get; set; }  
    public  string Title { get; set; }
    public  List<string> Categories { get; set; }

    public  int Price { get; set; }
    public  DateOnly StartDate { get; set; }
    public  DateOnly EndDate { get; set; }
    public string? Description { get; set; }
    public double Rating { get; set; } = 0;
    
    public  List<string> Locations { get; set; }
    public  List<string> Images { get; set; }
    public  int Status { get; set; } 
    public  int AvailableSets { get; set; }


}
