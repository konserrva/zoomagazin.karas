using System.ComponentModel.DataAnnotations;

namespace ZooShopApi.Models;

/// <summary>Данные для добавления нового товара.</summary>
public class CreateProductRequest
{
    [Required(ErrorMessage = "Укажите название товара")]
    [StringLength(100, MinimumLength = 2,
        ErrorMessage = "Название — от 2 до 100 символов")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Укажите категорию")]
    public string Category { get; set; } = "";

    [Range(1, 1_000_000, ErrorMessage = "Цена — от 1 до 1 000 000")]
    public decimal Price { get; set; }

    [Range(0, 100_000, ErrorMessage = "Остаток на складе — от 0 до 100 000")]
    public int Stock { get; set; }
}
