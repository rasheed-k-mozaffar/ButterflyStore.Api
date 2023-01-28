using Microsoft.AspNetCore.Mvc;

namespace ButterflyStore.Server.Controllers;

public class ProductsController : BaseController
{
    private readonly IProductsService _productsService;

    public ProductsController(IProductsService productsService)
    {
        _productsService = productsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productsService.GetAllProductsAsync();

        return Ok(products);
    }

    public async Task<IActionResult> AddProduct(ProductDto model)
    {

    }
}