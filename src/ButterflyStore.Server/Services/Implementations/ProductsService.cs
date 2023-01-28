namespace ButterflyStore.Server.Services.Implementations;

public class ProductsService : IProductsService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ProductsService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddProductAsync(ProductDto model)
    {
        //Create a new product using the given model.
        var itemToAdd = _mapper.Map<Product>(model);

        //Adding the product to the database and then persisting the changes.
        await _context.Products.AddAsync(itemToAdd);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProduct(int id)
    {
        //Find the requested item.
        var itemToDelete = await _context.Products.FindAsync(id);

        //If the item was found , then this will remove it and persist the changes.
        if (itemToDelete != null)
        {
            _context.Products.Remove(itemToDelete);
            await _context.SaveChangesAsync();
        }
        else //In case the item wasn't found.
        {
            throw new NotFoundException("No product was found with the given ID");
        }
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        //Get all the product records inside the products table and also include the category property
        //and ignore the changes cause we don't want to track anything since this will serve as a GET DATA Query.
        var products = await _context.Products
                        .Include(p => p.Category)
                        .AsNoTracking()
                        .ToListAsync();
        //Map all the products to PRODUCT DTOS and return them.
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto> GetProductByIdAsync(int id)
    {
        //Get the product , then map it to a product DTO and return it.
        var product = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

        return _mapper.Map<ProductDto>(product);

    }

    public Task Update(int id)
    {
        throw new NotImplementedException();
    }
}