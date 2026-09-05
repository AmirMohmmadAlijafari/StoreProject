using StoreProject.Application.DTOs.Product;
using StoreProject.Domain.Entities;

namespace StoreProject.Application.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();

    Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        ProductFilterDto? filter = null);

    Task<Product?> GetByIdAsync(Guid id);

    Task<Product> AddAsync(Product product);

    Task UpdateAsync(Product product);

    Task DeleteAsync(Product product);
}