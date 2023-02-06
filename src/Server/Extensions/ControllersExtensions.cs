using System;
namespace ButterflyStore.Server.Extensions
{
    public static class ControllersExtensions
    {

        /// <summary>
        /// This method builds a Product from the productDTO calling it.
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public static Product ConstructProduct(this ProductDto productDto)
        {
            return new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Color = productDto.Color,
                Size = productDto.Size,
                ImageUrl = productDto.ImageUrl,
                Price = productDto.Price,
                CategoryId = productDto.CategoryId
            };
        }


        /// <summary>
        /// This method buils a ProductDTO from the product calling it.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public static ProductDto ConstructProductDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name!,
                Description = product.Description,
                Color = product.Color,
                Size = product.Size,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CategoryName = product.Category!.Name
            };
        }


        /// <summary>
        /// This method builds a category from the categoryDTO calling it.
        /// </summary>
        /// <param name="categoryDto"></param>
        /// <returns></returns>
        public static Category ConstructCategory(this CategoryDto categoryDto)
        {
            return new Category
            {
                Name = categoryDto.Name!
            };
        }


        /// <summary>
        /// This method builds a categoryDTO from the category calling it.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static CategoryDto ConstructCategoryDto(this Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}

