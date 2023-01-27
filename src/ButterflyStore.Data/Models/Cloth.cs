using ButterflyStore.Data.Enums;

namespace ButterflyStore.Data.Models;

public class Cloth : ProductBase
{
    public string Color { get; set; } = null!;
    public SizeEnum Size { get; set; }
}