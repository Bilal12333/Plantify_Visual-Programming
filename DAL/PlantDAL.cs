using Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class PlantDAL
    {
        private static readonly IMongoCollection<PlantModel> _plantCollectioon = MongoHelper.GetProductCollection;

        public static async Task<List<PlantModel>> GetProductsAsync()
        {
            return await _plantCollectioon.Find(_ => true).ToListAsync();
        }
        public static async Task AddProductAsync(PlantModel plant)
        {
            await _plantCollectioon.InsertOneAsync(plant);
        }

        public static async Task DeleteProductAsync(string id)
        {
            var filter = Builders<PlantModel>.Filter.Eq(p => p.ProID, id);
            await _plantCollectioon.DeleteOneAsync(filter);
        }

        public static async Task<PlantModel> GetProductByIdAsync(string id)
        {
            var filter = Builders<PlantModel>.Filter.Eq(p => p.ProID, id);
            return await _plantCollectioon.Find(filter).FirstOrDefaultAsync();
        }


        public static async Task<List<PlantModel>> GetProductsByCategoryAsync(string category)
        {
            var filter = Builders<PlantModel>.Filter.Eq(p => p.Category, category);
            return await _plantCollectioon.Find(filter).ToListAsync();
        }

        public static async Task<bool> UpdateProductAsync(PlantModel product)
        {
            var filter = Builders<PlantModel>.Filter.Eq(p => p.ProID, product.ProID);
            var result = await _plantCollectioon.ReplaceOneAsync(filter, product);
            return result.ModifiedCount > 0;
        }
        public static async Task<List<PlantModel>> GetProductsByBusinessIdAsync(string businessId)
        {
            var filter = Builders<PlantModel>.Filter.Eq(p => p.BusinessId, businessId);
            return await _plantCollectioon.Find(filter).ToListAsync();
        }

        public static async Task<List<PlantModel>> GetPlantsByCategoryAsync(string category)
        {
            return await _plantCollectioon.Find(p => p.Category == category).ToListAsync();
        }


    }
}
