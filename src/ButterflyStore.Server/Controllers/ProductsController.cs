using Microsoft.AspNetCore.Mvc;

namespace ButterflyStore.Server.Controllers;

public class ProductsController : BaseController
{
    private readonly IProductsService _productsService;
    private readonly ICategoryService _categoryService;

    public ProductsController(IProductsService productsService, ICategoryService categoryService)
    {
        _productsService = productsService;
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productsService.GetAllProductsAsync();

        return Ok(products); //Return 200 OK STATUS CODE with all the products.
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(ProductDto model)
    {
        //Check if the model is valid.
        if (ModelState.IsValid)
        {
            //If the category name and the id are null , add it without creating a new category
            if (model.CategoryId == null)
            {
                var product = ConstructProduct(model);
                await _productsService.AddProductAsync(product);
                return Ok();
            }
            else //if category name and category id aren't null , then add the product and create a new category.
            {
                // Check if the category is existing 
                var category = await _categoryService.GetCategoryByIdAsync(model.CategoryId.Value);
                if (category == null)
                {
                    // Either return error or add it if the category name is not null 
                    return BadRequest("Category was not found");
                }
                else
                {
                    var product = ConstructProduct(model);
                    await _productsService.AddProductAsync(product);
                }
                return Ok();
            }
        }

        //Return the validations with a 400 STATUS CODE
        return BadRequest(ModelState);
    }

    private Product ConstructProduct(ProductDto model)
    {
        return new Product()
        {
            Name = model.Name,
            Color = model.Color,
            Description = model.Description,
            ImageUrl = model.ImageUrl,
            Price = model.Price,
            Size = model.Size,
            CategoryId = model.CategoryId,
        };
    }
}