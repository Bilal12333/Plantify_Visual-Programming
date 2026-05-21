using Entities;
using DAL;

namespace PlantifyAdmin.Services
{
    public class CartService : ICartService
    {
        public async Task AddCardToCollection(string UserId)
        {
            await CartDAL.AddCardToCollection(UserId);
        }

        public async Task AddPlantToCartAsync(string UserId, List<PlantModel> plant)
        {
            await CartDAL.AddPlantToCartAsync(UserId, plant);
        }

        public async Task ClearCartAsync(string UserId)
        {
            await CartDAL.ClearCartAsync(UserId);
        }

        public async Task<List<CartModel>> GetCartAsync()
        {
            return await CartDAL.GetCartAsync();
        }

        public async Task RemovePlantFromCartAsync(string UserId, string plantId)
        {
            await CartDAL.RemovePlantFromCartAsync(UserId, plantId);
        }
    }
}
