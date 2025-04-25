using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs.TripDTOs;

public record class UpdateTripDTO
{
    [StringLength(100)]
    public string? Title { get; set; }

    [Range(0, int.MaxValue)]
    public int? Price { get; set; }

    public DateTime? StartDate { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Range(0, 5)]
    public double? Rating { get; set; } = 0;

    public List<int>? LocationIds { get; set; }

    public string[]? Images { get; set; }

    public int? Status { get; set; }

    [Range(0, int.MaxValue)]
    public int? AvailableSets { get; set; }

    public List<int>? CategoryIds { get; set; }
}
