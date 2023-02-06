namespace ButterflyStore.Server.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category> GetCategoryByIdAsync(int id);
    IEnumerable<Product> GetProductsByCategory(Category category);
    Task AddCategoryAsync(Category model);
    Task DeleteCategory(int id);
    Task Update(int id);
}