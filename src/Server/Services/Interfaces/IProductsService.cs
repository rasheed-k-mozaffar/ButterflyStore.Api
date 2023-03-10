namespace ButterflyStore.Server.Services.Interfaces;

public interface IProductsService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product> GetProductByIdAsync(int id);
    Task AddProductAsync(Product model);
    Task DeleteProduct(int id);
    Task Update(int id);
}