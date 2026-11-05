using Microsoft.AspNetCore.Mvc;
using ZooShopApi.Data;
using ZooShopApi.Models;

namespace ZooShopApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        IProductRepository repository,
        ILogger<ProductsController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    // GET /api/products
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetAll(
        CancellationToken cancellationToken)
    {
        var items = await _repository.GetAllAsync(cancellationToken);
        return Ok(items);
    }

    // GET /api/products/2
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetById(
        int id, CancellationToken cancellationToken)
    {
        Product? item =
            await _repository.GetByIdAsync(id, cancellationToken);
        if (item is null)
            return NotFound();

        return Ok(item);
    }

    // POST /api/products
    [HttpPost]
    public async Task<ActionResult<Product>> Create(
        CreateProductRequest request, CancellationToken cancellationToken)
    {
        // Бизнес-правило: название товара должно быть уникальным -> 409.
        // Проверяем до сохранения.
        IReadOnlyList<Product> all =
            await _repository.GetAllAsync(cancellationToken);
        bool nameExists = all.Any(p => string.Equals(
            p.Name.Trim(), request.Name.Trim(),
            StringComparison.OrdinalIgnoreCase));
        if (nameExists)
            return Conflict($"Товар с названием «{request.Name}» уже есть");

        var product = new Product
        {
            Name = request.Name,
            Category = request.Category,
            Price = request.Price,
            Stock = request.Stock,
        };

        Product created =
            await _repository.AddAsync(product, cancellationToken);
        _logger.LogInformation("Добавлен товар {ProductId}", created.Id);

        return CreatedAtAction(
            nameof(GetById), new { id = created.Id }, created);
    }

    // DELETE /api/products/2
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        int id, CancellationToken cancellationToken)
    {
        bool deleted = await _repository.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
