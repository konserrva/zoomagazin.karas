using ZooShopApi.Models;

namespace ZooShopApi.Data;

/// <summary>Хранилище товаров в памяти (до подключения базы данных).</summary>
public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _items = new()
    {
        new() { Id = 1, Name = "Сухой корм для собак «Pro Dog» 15 кг",
                Category = "Корма", Price = 3490m, Stock = 24 },
        new() { Id = 2, Name = "Наполнитель для кошачьего туалета 10 л",
                Category = "Гигиена", Price = 650m, Stock = 40 },
        new() { Id = 3, Name = "Поводок-рулетка 5 м",
                Category = "Аксессуары", Price = 890m, Stock = 15 },
    };

    private readonly object _sync = new();
    private int _nextId = 4;

    public Task<IReadOnlyList<Product>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        lock (_sync)
        {
            IReadOnlyList<Product> copy = _items.ToList();
            return Task.FromResult(copy);
        }
    }

    public Task<Product?> GetByIdAsync(
        int id, CancellationToken cancellationToken = default)
    {
        lock (_sync)
        {
            return Task.FromResult(_items.FirstOrDefault(p => p.Id == id));
        }
    }

    public Task<Product> AddAsync(
        Product product, CancellationToken cancellationToken = default)
    {
        lock (_sync)
        {
            product.Id = _nextId++;
            _items.Add(product);
            return Task.FromResult(product);
        }
    }

    public Task<bool> DeleteAsync(
        int id, CancellationToken cancellationToken = default)
    {
        lock (_sync)
        {
            int removed = _items.RemoveAll(p => p.Id == id);
            return Task.FromResult(removed > 0);
        }
    }
}
