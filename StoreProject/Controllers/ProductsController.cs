using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using StoreProject.Application.DTOs.Product;
using StoreProject.Application.Interfaces;

namespace StoreProject.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IValidator<CreateProductDto> _createValidator;
    private readonly IValidator<UpdateProductDto> _updateValidator;

    public ProductsController(
        IProductService productService,
        IValidator<CreateProductDto> createValidator,
        IValidator<UpdateProductDto> updateValidator)
    {
        _productService = productService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    // GET: api/products
    [HttpGet]
    [EnableQuery]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] ProductFilterDto? filter = null)
    {
        if (page < 1)
        {
            return BadRequest(new
            {
                message = "شماره صفحه باید بزرگ‌تر از صفر باشد."
            });
        }

        if (pageSize < 1 || pageSize > 100)
        {
            return BadRequest(new
            {
                message = "اندازه صفحه باید بین 1 تا 100 باشد."
            });
        }

        if (filter?.MinPrice < 0)
        {
            return BadRequest(new
            {
                message = "حداقل قیمت نمی‌تواند منفی باشد."
            });
        }

        if (filter?.MaxPrice < 0)
        {
            return BadRequest(new
            {
                message = "حداکثر قیمت نمی‌تواند منفی باشد."
            });
        }

        if (filter?.MinPrice is not null &&
            filter?.MaxPrice is not null &&
            filter.MinPrice > filter.MaxPrice)
        {
            return BadRequest(new
            {
                message = "حداقل قیمت نمی‌تواند بیشتر از حداکثر قیمت باشد."
            });
        }

        // Validate sorting field
        if (!string.IsNullOrWhiteSpace(filter?.SortBy) &&
            !string.Equals(filter.SortBy, "name", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(filter.SortBy, "price", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(new
            {
                message = "مرتب‌سازی فقط بر اساس نام یا قیمت امکان‌پذیر است."
            });
        }

        // Validate sorting order
        if (!string.IsNullOrWhiteSpace(filter?.SortOrder) &&
            !string.Equals(filter.SortOrder, "asc", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(filter.SortOrder, "desc", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(new
            {
                message = "نوع مرتب‌سازی باید asc یا desc باشد."
            });
        }

        var products = await _productService.GetAllAsync(
            page,
            pageSize,
            filter);

        if (!products.Items.Any())
        {
            return NotFound(new
            {
                message = "محصول مورد نظر پیدا نشد."
            });
        }

        return Ok(products);
    }

    // GET: api/products/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product is null)
        {
            return NotFound(new
            {
                message = "محصول مورد نظر پیدا نشد."
            });
        }

        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        var validationResult =
            await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                message = "اطلاعات وارد شده معتبر نیست.",
                errors = validationResult.Errors.Select(error => new
                {
                    field = error.PropertyName,
                    message = error.ErrorMessage
                })
            });
        }

        var product = await _productService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product);
    }

    // PUT: api/products/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateProductDto dto)
    {
        var validationResult =
            await _updateValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                message = "اطلاعات وارد شده معتبر نیست.",
                errors = validationResult.Errors.Select(error => new
                {
                    field = error.PropertyName,
                    message = error.ErrorMessage
                })
            });
        }

        var updated = await _productService.UpdateAsync(id, dto);

        if (!updated)
        {
            return NotFound(new
            {
                message = "محصول مورد نظر پیدا نشد."
            });
        }

        return NoContent();
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _productService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "محصول مورد نظر پیدا نشد."
            });
        }

        return NoContent();
    }
}