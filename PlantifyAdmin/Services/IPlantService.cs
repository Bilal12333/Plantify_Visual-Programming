using Entities;

namespace PlantifyAdmin.Services
{
    public interface IPlantService
    {
        Task<List<PlantModel>> GetProductsAsync();
        Task AddProductAsync(PlantModel plant);
        Task DeleteProductAsync(string id);
        Task<PlantModel> GetProductByIdAsync(string id);
        Task<List<PlantModel>> GetProductsByCategoryAsync(string category);
        Task<bool> UpdateProductAsync(PlantModel product);
        Task<List<PlantModel>> GetProductsByBusinessIdAsync(string businessId);
        Task<List<PlantModel>> GetPlantsByCategoryAsync(string category);
    }
}
