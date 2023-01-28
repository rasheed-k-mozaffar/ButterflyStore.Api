namespace ButterflyStore.Server.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    Task<CategoryDto> GetCategoryByIdAsync(int id);
    Task AddCategoryAsync(CategoryDto model);
    Task DeleteCategory(int id);
    Task Update(int id);
}