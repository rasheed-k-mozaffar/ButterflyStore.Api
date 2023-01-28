namespace ButterflyStore.Server.Profiles;

public class CategoriesProfile : Profile
{
    public CategoriesProfile()
    {
        //Source -> Target.
        CreateMap<Category, CategoryDto>().ReverseMap();
    }
}