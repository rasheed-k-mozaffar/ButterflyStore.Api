using Microsoft.AspNetCore.Mvc;

namespace ButterflyStore.Server.Controllers;

public class CategoriesController : BaseController
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();

        return Ok(categories); //Return 200 OK STATUS CODE.
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


    private Category ConstructCategory(CategoryDto model)
    {
        return new Category()
        {
            Name = model.Name!
        };
    }
}