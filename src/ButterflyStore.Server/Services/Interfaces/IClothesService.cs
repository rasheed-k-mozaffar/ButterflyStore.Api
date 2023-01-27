namespace ButterflyStore.Server.Services.Interfaces;

public interface IClothesService
{
    Task<IEnumerable<ClothDto>> GetAllAsync();
    Task<ClothDto> GetById(int id);
    Task Add(ClothDto model);
    Task Delete(int id);
    Task Update(int id);
}