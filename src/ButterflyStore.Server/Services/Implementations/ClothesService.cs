namespace ButterflyStore.Server.Services.Implementations;

public class ClothesService : IClothesService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ClothesService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task Add(ClothDto model)
    {
        //Mapping the given DTO to a cloth item to add it to the clothes table and them persisting the changes.
        var itemToAdd = _mapper.Map<Cloth>(model);

        await _context.Clothes.AddAsync(itemToAdd);
        await _context.SaveChangesAsync();

    }

    public async Task Delete(int id)
    {
        //Finding the item by its ID.
        var itemToDelete = await _context.Clothes.FindAsync(id);

        //In case of NOT NULL , Remove the item
        if (itemToDelete != null)
        {
            _context.Clothes.Remove(itemToDelete);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new NotFoundException("No item was found with the given ID");
        }
    }

    public async Task<IEnumerable<ClothDto>> GetAllAsync()
    {
        //Getting all the entites from the Clothes table without tracking 
        //them cause this query will not modify any record.
        var clothes = await _context.Clothes.AsNoTracking().ToListAsync();
        return _mapper.Map<IEnumerable<ClothDto>>(clothes);
    }

    public async Task<ClothDto> GetById(int id)
    {
        //Getting a specific item from the Clothes table by its ID
        var cloth = await _context.Clothes.FindAsync(id);
        return _mapper.Map<ClothDto>(cloth);
    }

    public Task Update(int id)
    {
        throw new NotImplementedException();
    }
}