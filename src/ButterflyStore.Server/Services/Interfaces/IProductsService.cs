namespace ButterflyStore.Server.Services.Interfaces;

public interface IProductsService
{
    Task<IEnumerable<ProductDto>> GetAll();
    Task<ProductDto> GetProductById(int id);
    Task Add(ProductDto model);
    Task Delete(int id);
    Task Update(int id);
}