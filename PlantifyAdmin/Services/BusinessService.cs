using DAL;
using Entities;

namespace PlantifyAdmin.Services
{
    public class BusinessService : IBusinessService
    {
        public async Task AddBusinessAsync(BusinessModel business)
        {
            if (string.IsNullOrWhiteSpace(business.ShopName))
              throw new ArgumentException("Shop Name is required.");

            if (string.IsNullOrWhiteSpace(business.Phone))
              throw new ArgumentException("Phone number is required.");

            await BusinessDAL.AddBusinessAsync(business);
        }

        public async Task<List<BusinessModel>> GetAllBusinessesAsync()
        {
            return await BusinessDAL.GetAllBusinessesAsync();
        }

        public async Task<BusinessModel?> GetBusinessByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Business Id is required.");

            return await BusinessDAL.GetBusinessByIdAsync(id);
        }

        public async Task UpdateBusinessAsync(BusinessModel business)
        {
            if (string.IsNullOrWhiteSpace(business.Id))
                throw new ArgumentException("Business Id is required for update.");

            await BusinessDAL.UpdateBusinessAsync(business);
        }

        public async Task DeleteBusinessAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Business Id is required for deletion.");

            await BusinessDAL.DeleteBusinessAsync(id);
        }

        public async Task<List<BusinessModel>> GetBusinessesByOwnerIdAsync(string ownerId)
        {
            if (string.IsNullOrWhiteSpace(ownerId))
                throw new ArgumentException("OwnerId is required.");

            return await BusinessDAL.GetBusinessesByOwnerIdAsync(ownerId);
        }
    }
}
