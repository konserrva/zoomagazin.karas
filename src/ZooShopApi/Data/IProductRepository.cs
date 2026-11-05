using ZooShopApi.Models;

namespace ZooShopApi.Data;

/// <summary>Хранилище товаров.</summary>
public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<Product?> GetByIdAsync(
        int id, CancellationToken cancellationToken = default);

    Task<Product> AddAsync(
        Product product, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
        int id, CancellationToken cancellationToken = default);
}
