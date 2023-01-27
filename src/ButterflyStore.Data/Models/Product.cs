using ButterflyStore.Data.Enums;

namespace ButterflyStore.Data.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public decimal Price { get; set; }
    public string Color { get; set; } = null!;
    public SizeEnum Size { get; set; }

    //Navigations And Relations
    public Category? Category { get; set; }
    public int? CategoryId { get; set; }
}