using AutoMapper;

namespace ButterflyStore.Server.Profiles;

public class ClothesProfile : Profile
{
    public ClothesProfile()
    {
        //Source -> Target
        CreateMap<ClothDto, Cloth>().ReverseMap();
    }

}