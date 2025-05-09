using Backend.Models;

namespace Backend.DTOs.CategoryDTOs;

public record class CreateCategoryDTO
{
    public required int Id { get; set; }
    public required string Name { get; set; }

}
