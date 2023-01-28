namespace ButterflyStore.Server.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CategoryService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddCategoryAsync(CategoryDto model)
    {
        //Create the category from the recieved DTO.
        var categoryToAdd = _mapper.Map<Category>(model);

        //Add the category and persist the changes.
        await _context.Categories.AddAsync(categoryToAdd);
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

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        //Get all the records from the categories table.
        var categories = await _context.Categories
                        .AsNoTracking()
                        .ToListAsync();

        //Return a collection of Category DTOs.
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<CategoryDto> GetCategoryByIdAsync(int id)
    {
        //Get the category by its ID without tracking its changes.
        var category = await _context.Categories
                             .AsNoTracking()
                             .FirstOrDefaultAsync(c => c.Id == id);

        return _mapper.Map<CategoryDto>(category);
    }

    public Task Update(int id)
    {
        throw new NotImplementedException();
    }
}