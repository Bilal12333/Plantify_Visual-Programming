using Entities;

namespace PlantifyAdmin.Services
{
    public interface IBusinessService
    {
        Task AddBusinessAsync(BusinessModel business);
        Task<List<BusinessModel>> GetAllBusinessesAsync();
        Task<BusinessModel?> GetBusinessByIdAsync(string id);
        Task UpdateBusinessAsync(BusinessModel business);
        Task DeleteBusinessAsync(string id);
        Task<List<BusinessModel>> GetBusinessesByOwnerIdAsync(string ownerId);
    }
}
