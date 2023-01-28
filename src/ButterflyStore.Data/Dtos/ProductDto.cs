using ButterflyStore.Data.Enums;
using ButterflyStore.Data.Models;

namespace ButterflyStore.Data.Dtos;

public class ProductDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public decimal Price { get; set; }
    public string Color { get; set; } = null!;
    public SizeEnum Size { get; set; }
    public string? CategoryName { get; set; }
    public int? CategoryId { get; set; }
}