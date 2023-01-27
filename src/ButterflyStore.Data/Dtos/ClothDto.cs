using ButterflyStore.Data.Enums;

namespace ButterflyStore.Data.Dtos;

public class ClothDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public decimal Price { get; set; }
    public string Color { get; set; } = null!;
    public SizeEnum Size { get; set; }
}