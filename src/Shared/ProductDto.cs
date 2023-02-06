using ButterflyStore.Shared.Enums;

namespace ButterflyStore.Shared;
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public decimal Price { get; set; }
    public string Color { get; set; } = null!;
    public Size Size { get; set; }
    public string? CategoryName { get; set; }
    public int? CategoryId { get; set; }
}

