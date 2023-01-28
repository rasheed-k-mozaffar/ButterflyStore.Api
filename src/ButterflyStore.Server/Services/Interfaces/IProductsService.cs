namespace ButterflyStore.Server.Services.Interfaces;

public interface IProductsService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductDto> GetProductByIdAsync(int id);
    Task AddProductAsync(ProductDto model);
    Task DeleteProduct(int id);
    Task Update(int id);
}