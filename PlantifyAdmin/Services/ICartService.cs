using Entities;

namespace PlantifyAdmin.Services
{
    public interface ICartService
    {
        Task<List<CartModel>> GetCartAsync();
        Task AddPlantToCartAsync(string UserId, List<PlantModel> plant);
        Task AddCardToCollection(string UserId);
        Task RemovePlantFromCartAsync(string UserId, string plantId);
        Task ClearCartAsync(string UserId);
    }
}
