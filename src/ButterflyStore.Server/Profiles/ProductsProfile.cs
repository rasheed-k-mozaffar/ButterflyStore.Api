namespace ButterflyStore.Server.Profiles;

public class ProductsProfile : Profile
{
    public ProductsProfile()
    {
        //Source -> Target.
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}