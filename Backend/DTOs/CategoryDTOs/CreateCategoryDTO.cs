using Backend.Models;

namespace Backend.DTOs.CategoryDTOs;

public record class CreateCategoryDTO
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    //mappers
    public static implicit operator Category(CreateCategoryDTO dto) => new Category
    {
        Id = dto.Id,
        Name = dto.Name,
    };
    public static implicit operator CreateCategoryDTO(Category category) => new CreateCategoryDTO
    {
        Id = category.Id,
        Name = category.Name,
    };

}
