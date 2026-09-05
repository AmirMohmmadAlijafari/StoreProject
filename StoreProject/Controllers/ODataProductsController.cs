using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreProject.Infrastructure.Data;

namespace StoreProject.Api.Controllers;

[ApiController]
[Route("odata/products")]
public class ODataProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ODataProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [EnableQuery]
    public IQueryable GetProducts()
    {
        return _context.Products
            .AsNoTracking();
    }
}