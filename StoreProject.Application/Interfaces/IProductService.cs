using StoreProject.Application.DTOs.Common;
using StoreProject.Application.DTOs.Product;

namespace StoreProject.Application.Interfaces;

public interface IProductService
{
    Task<PagedResultDto<ProductDto>> GetAllAsync(
        int page,
        int pageSize,
        ProductFilterDto? filter = null);

    Task<ProductDto?> GetByIdAsync(Guid id);

    Task<ProductDto> CreateAsync(CreateProductDto dto);

    Task<bool> UpdateAsync(Guid id, UpdateProductDto dto);

    Task<bool> DeleteAsync(Guid id);
}