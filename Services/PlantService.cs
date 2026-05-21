using Entities;
using DAL;

namespace PlantifyAdmin.Services
{
    public class PlantService : IPlantService
    {
        public async Task AddProductAsync(PlantModel plant)
        {
            await PlantDAL.AddProductAsync(plant);
        }

        public async Task DeleteProductAsync(string id)
        {
            await PlantDAL.DeleteProductAsync(id);
        }

        public async Task<PlantModel> GetProductByIdAsync(string id)
        {
            return await PlantDAL.GetProductByIdAsync(id);
        }

        public async Task<List<PlantModel>> GetProductsAsync()
        {
            return await PlantDAL.GetProductsAsync();
        }

        public async Task<List<PlantModel>> GetProductsByCategoryAsync(string category)
        {
            return await PlantDAL.GetProductsByCategoryAsync(category);
        }

        public async Task<bool> UpdateProductAsync(PlantModel product)
        {
            return await PlantDAL.UpdateProductAsync(product);
        }
        public async Task<List<PlantModel>> GetProductsByBusinessIdAsync(string businessId)
        {
            return await PlantDAL.GetProductsByBusinessIdAsync(businessId);
        }
        public async Task<List<PlantModel>> GetPlantsByCategoryAsync(string category)
        {
            return await PlantDAL.GetPlantsByCategoryAsync(category);
        }

    }
}
