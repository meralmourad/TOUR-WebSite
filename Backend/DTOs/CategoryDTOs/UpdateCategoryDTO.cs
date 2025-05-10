using Backend.Models;

namespace Backend.DTOs.CategoryDTOs;

public record class UpdateCategoryDTO
{
    public required int Id { get; set; }
    public string? Name { get; set; } = string.Empty;

}
