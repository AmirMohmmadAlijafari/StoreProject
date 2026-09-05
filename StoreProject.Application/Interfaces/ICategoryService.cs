using StoreProject.Application.DTOs.Category;

namespace StoreProject.Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync();

    Task<CategoryDto?> GetByIdAsync(Guid id);

    Task<CategoryDto> CreateAsync(CreateCategoryDto dto);

    Task<bool> UpdateAsync(Guid id, UpdateCategoryDto dto);

    Task<bool> DeleteAsync(Guid id);
}