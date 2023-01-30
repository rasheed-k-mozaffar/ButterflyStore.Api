using Microsoft.AspNetCore.Mvc;

namespace ButterflyStore.Server.Controllers;

public class CategoriesController : BaseController
{
    private readonly ICategoryService _categoryService;
    private readonly AppDbContext _context;

    public CategoriesController(ICategoryService categoryService, AppDbContext context)
    {
        _categoryService = categoryService;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();

        return Ok(categories.Select(c => ConstructCategoryDto(c))); //Return 200 OK STATUS CODE.
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
        {
            return BadRequest("0 is not a valid ID");
        }

        var category = await _categoryService.GetCategoryByIdAsync(id);

        if (category == null)
        {
            return NotFound($"No category was found with the ID: {id}");
        }

        //Build a category DTO from the retrieved category.
        var categoryAsDto = ConstructCategoryDto(category);

        return Ok(categoryAsDto); //Return 200 OK STATUS CODE.
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory(CategoryDto model)
    {
        if (ModelState.IsValid)
        {
            var category = ConstructCategory(model);
            await _categoryService.AddCategoryAsync(category);
            return NoContent(); //Return 204 NO CONTENT STATUS CODE.
        }

        return BadRequest(); //Return 400 STATUS CODE.
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        //In case the ID is 0 , return a 400 STATUS CODE.
        if (id == 0)
        {
            return BadRequest("0 is not a valid ID");
        }

        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound($"No category was found with the ID: {id}");
        }

        await _categoryService.DeleteCategory(id);

        return NoContent(); //Return 204 NO CONTENT.
    }

    // TODO : Needs Checking.
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, CategoryDto model)
    {
        if (id == 0)
        {
            return BadRequest("0 is not a valid ID");
        }

        var categoryToUpdate = await _categoryService.GetCategoryByIdAsync(id);

        if (categoryToUpdate == null)
        {
            return NotFound($"No category was found with the ID: {id}");
        }

        if (ModelState.IsValid)
        {
            categoryToUpdate.Name = model.Name!;

            await _context.SaveChangesAsync();

            return Ok(categoryToUpdate); //Return 204 NO CONTENT STATUS CODE.
        }

        return BadRequest();
    }


    private Category ConstructCategory(CategoryDto model)
    {
        return new Category()
        {
            Name = model.Name!
        };
    }
    private CategoryDto ConstructCategoryDto(Category model)
    {
        return new CategoryDto()
        {
            Id = model.Id,
            Name = model.Name
        };
    }
}