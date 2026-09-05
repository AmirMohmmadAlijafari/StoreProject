using Microsoft.EntityFrameworkCore;
using StoreProject.Application.DTOs.Product;
using StoreProject.Application.Interfaces;
using StoreProject.Domain.Entities;
using StoreProject.Infrastructure.Data;

namespace StoreProject.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        ProductFilterDto? filter = null)
    {
        var query = _context.Products
            .AsNoTracking()
            .AsQueryable();

        // Filtering by name
        if (!string.IsNullOrWhiteSpace(filter?.Name))
        {
            query = query.Where(p =>
                p.Name.ToLower().Contains(filter.Name.ToLower()));
        }

        // Filtering by minimum price
        if (filter?.MinPrice is not null)
        {
            query = query.Where(p =>
                p.Price >= filter.MinPrice.Value);
        }

        // Filtering by maximum price
        if (filter?.MaxPrice is not null)
        {
            query = query.Where(p =>
                p.Price <= filter.MaxPrice.Value);
        }

        // Filtering by active status
        if (filter?.IsActive is not null)
        {
            query = query.Where(p =>
                p.IsActive == filter.IsActive.Value);
        }

        // Sorting
        if (string.Equals(
                filter?.SortBy,
                "name",
                StringComparison.OrdinalIgnoreCase))
        {
            if (string.Equals(
                    filter?.SortOrder,
                    "desc",
                    StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderByDescending(p => p.Name);
            }
            else
            {
                query = query.OrderBy(p => p.Name);
            }
        }
        else if (string.Equals(
                     filter?.SortBy,
                     "price",
                     StringComparison.OrdinalIgnoreCase))
        {
            if (string.Equals(
                    filter?.SortOrder,
                    "desc",
                    StringComparison.OrdinalIgnoreCase))
            {
                query = query.OrderByDescending(p => p.Price);
            }
            else
            {
                query = query.OrderBy(p => p.Price);
            }
        }
        else
        {
            // Default sorting
            query = query.OrderBy(p => p.CreatedAt);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}