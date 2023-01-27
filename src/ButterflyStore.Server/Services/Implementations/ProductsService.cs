namespace ButterflyStore.Server.Services.Implementations;

public class ProductsService : IProductsService
{
    private readonly AppDbContext _context;

    public ProductsService(AppDbContext context)
    {
        _context = context;
    }

    public Task Add(ProductDto model)
    {
        throw new NotImplementedException();
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ProductDto>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<ProductDto> GetProductById(int id)
    {
        throw new NotImplementedException();
    }

    public Task Update(int id)
    {
        throw new NotImplementedException();
    }
}