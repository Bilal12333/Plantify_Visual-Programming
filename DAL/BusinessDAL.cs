using Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL
{
    public static class BusinessDAL
    {
        private static readonly IMongoCollection<BusinessModel> _businessCollection = MongoHelper.GetBusinessCollection;

        public static async Task AddBusinessAsync(BusinessModel business)
        {
            await _businessCollection.InsertOneAsync(business);
        }

        public static async Task<List<BusinessModel>> GetAllBusinessesAsync()
        {
            return await _businessCollection.Find(_ => true).ToListAsync();
        }

        public static async Task<BusinessModel?> GetBusinessByIdAsync(string id)
        {
            return await _businessCollection.Find(b => b.Id == id).FirstOrDefaultAsync();
        }

        public static async Task UpdateBusinessAsync(BusinessModel business)
        {
            var filter = Builders<BusinessModel>.Filter.Eq(b => b.Id, business.Id);
            await _businessCollection.ReplaceOneAsync(filter, business);
        }

        public static async Task DeleteBusinessAsync(string id)
        {
            var filter = Builders<BusinessModel>.Filter.Eq(b => b.Id, id);
            await _businessCollection.DeleteOneAsync(filter);
        }

        public static async Task<List<BusinessModel>> GetBusinessesByOwnerIdAsync(string ownerId)
        {
            var filter = Builders<BusinessModel>.Filter.Eq("OwnerId", ownerId);
            return await _businessCollection.Find(filter).ToListAsync();
        }
    }
}
