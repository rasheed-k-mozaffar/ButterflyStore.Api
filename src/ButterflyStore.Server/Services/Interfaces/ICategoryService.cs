namespace ButterflyStore.Server.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category> GetCategoryByIdAsync(int id);
    Task AddCategoryAsync(Category model);
    Task DeleteCategory(int id);
    Task Update(int id);
}