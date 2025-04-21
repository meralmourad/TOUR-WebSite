using Backend.Models;

namespace Backend.DTOs.CategoryDTOs;

public record class UpdateCategoryDTO
{
    public required int Id { get; set; }
    public string? Name { get; set; } = string.Empty;

    //mappers
    public static implicit operator Category(UpdateCategoryDTO dto) => new Category
    {
        Id = dto.Id,
        Name = dto.Name ?? string.Empty,
    };
    public static implicit operator UpdateCategoryDTO(Category category) => new UpdateCategoryDTO
    {
        Id = category.Id,
        Name = category.Name,
    };
}
