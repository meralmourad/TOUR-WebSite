namespace Backend.DTOs;

public record class searchResDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = "Tourist";
}
