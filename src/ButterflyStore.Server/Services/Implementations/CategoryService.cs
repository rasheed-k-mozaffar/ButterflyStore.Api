namespace ButterflyStore.Server.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
        
    }

    public async Task AddCategoryAsync(Category model)
    {

        await _context.Categories.AddAsync(model);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategory(int id)
    {
        //Find the category by its ID.
        var categoryToDelete = await _context.Categories.FindAsync(id);

        //Delete the category if it's found
        if (categoryToDelete != null)
        {
            _context.Categories.Remove(categoryToDelete);
            await _context.SaveChangesAsync();
        }
        else //In case nothing was found with the given ID.
        {
            throw new NotFoundException("No category was found with the given ID");
        }
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        //Get all the records from the categories table.
        var categories = await _context.Categories
                        .AsNoTracking()
                        .ToListAsync();

        //Return a collection of Categories.
        return categories;
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        //Get the category by its ID without tracking its changes.
        var category = await _context.Categories
                             .AsNoTracking()
                             .FirstOrDefaultAsync(c => c.Id == id);

        return category!;
    }

    public IEnumerable<Product> GetProductsByCategory(Category category)
    {
        //Get the products
        var productsByCategory = from product in _context.Products.Include(p => p.Category)
                                 where product.CategoryId == category.Id
                                
                                 orderby product.Name
                                 select product;

        return productsByCategory;
    }

    public Task Update(int id)
    {
        throw new NotImplementedException();
    }
}