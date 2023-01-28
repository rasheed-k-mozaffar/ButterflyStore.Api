using Microsoft.AspNetCore.Mvc;

namespace ButterflyStore.Server.Controllers;

public class ProductsController : BaseController
{
    private readonly IProductsService _productsService;
    private readonly ICategoryService _categoryService;
    private readonly AppDbContext _context;

    public ProductsController(IProductsService productsService, ICategoryService categoryService, AppDbContext context)
    {
        _productsService = productsService;
        _categoryService = categoryService;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productsService.GetAllProductsAsync();
        var productsAsDtos = new List<ProductDto>();

        foreach (Product product in products)
        {
            var productDto = ConstructProductDto(product);
            productsAsDtos.Add(productDto);
        }

        return Ok(productsAsDtos); //Return 200 OK STATUS CODE with all the products.
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productsService.GetProductByIdAsync(id);

        //Check if a product exists with the given ID.
        if (product == null)
        {
            return BadRequest($"No product was found with the ID: {id}");
        }

        //Build a DTO from the retrieved product and send it.
        var productAsDto = ConstructProductDto(product);

        return Ok(productAsDto);

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
                    //Build a product object and pass it to the PRODUCT SERVICE.
                    var product = ConstructProduct(model);
                    await _productsService.AddProductAsync(product);
                }
                return Ok();
            }
        }

        //Return the validations with a 400 STATUS CODE
        return BadRequest(ModelState);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        //In case the ID is 0 , return a 400 STATUS CODE.
        if (id == 0)
        {
            return BadRequest("0 is not a valid ID");
        }

        await _productsService.DeleteProduct(id);

        return NoContent(); //Return 204 NO CONTENT.
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, ProductDto model)
    {
        if (id == 0)
        {
            return BadRequest("0 is not a valid ID");
        }

        if (ModelState.IsValid)
        {
            var productToUpdate = await _productsService.GetProductByIdAsync(id);
            productToUpdate.Name = model.Name;
            productToUpdate.Color = model.Color;
            productToUpdate.Description = model.Description;
            productToUpdate.Price = model.Price;
            productToUpdate.Size = model.Size;
            productToUpdate.CategoryId = model.CategoryId;
            productToUpdate.Category!.Name = model.CategoryName!;

            await _context.SaveChangesAsync();

            return NoContent(); //Return 200 NO CONTENT STATUS CODE.
        }

        return BadRequest(ModelState); //Return 400 BAD REQUEST STATUS CODE.
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
    private ProductDto ConstructProductDto(Product model)
    {
        return new ProductDto()
        {
            Id = model.Id,
            Name = model.Name!,
            Color = model.Color,
            Description = model.Description,
            ImageUrl = model.ImageUrl,
            Price = model.Price,
            Size = model.Size,
            CategoryId = model.CategoryId,
        };
    }

}