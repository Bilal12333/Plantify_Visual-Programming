using Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class CartDAL
    {
        private static readonly IMongoCollection<CartModel> _cartCollection = MongoHelper.GetCartCollection;
        private static readonly IMongoCollection<PlantModel> _productCollection = MongoHelper.GetProductCollection;
        public static async Task<List<CartModel>> GetCartAsync()
        {
            return await _cartCollection.Find(_ => true).ToListAsync();
        }

        public static async Task AddCardToCollection(string UserIdParameter)
        {
            if (string.IsNullOrEmpty(UserIdParameter))
                throw new ArgumentException("UserId cannot be null or empty.");

            // Check if a cart already exists for this user
            var filter = Builders<CartModel>.Filter.Eq(c => c.UserId, UserIdParameter);
            var existingCart = await _cartCollection.Find(filter).FirstOrDefaultAsync();

            if (existingCart == null)
            {
                await _cartCollection.InsertOneAsync(new CartModel
                {
                    UserId = UserIdParameter,
                    Plants = new()
                });
            }
        }
        public static async Task AddPlantToCartAsync(string UserId, List<PlantModel> plantList)
        {
            if (plantList == null || plantList.Count == 0)
                throw new ArgumentException("Plant list cannot be null or empty.");

            var filter = Builders<CartModel>.Filter.Eq(c => c.UserId, UserId);
            var cart = await _cartCollection.Find(filter).FirstOrDefaultAsync();

            if (cart == null)
            {
                // Create a new cart with the given plant list
                cart = new CartModel
                {
                    UserId = UserId,
                    Plants = plantList
                };
                await _cartCollection.InsertOneAsync(cart);
            }
            else
            {
                foreach (var plant in plantList)
                {
                    if (plant.Id == null)
                        throw new ArgumentException("Plant.Id cannot be null.");

                    // Check if plant already exists in cart
                    var existingPlant = cart.Plants.FirstOrDefault(p => p.Id == plant.Id);
                    if (existingPlant != null)
                    {
                        // Increase quantity
                        existingPlant.Quantity += plant.Quantity; // Make sure PlantModel has Quantity property
                    }
                    else
                    {
                        // Add new plant
                        cart.Plants.Add(plant);
                    }
                }

                // Update the cart in DB
                var update = Builders<CartModel>.Update.Set(c => c.Plants, cart.Plants);
                await _cartCollection.UpdateOneAsync(filter, update);
            }
        }

        public static async Task RemovePlantFromCartAsync(string UserId, string plantId)
        {
            var filter = Builders<CartModel>.Filter.Eq(c => c.UserId, UserId);
            var cart = await _cartCollection.Find(filter).FirstOrDefaultAsync();
            if (cart != null)
            {
                var plantToRemove = cart.Plants.FirstOrDefault(p => p.Id == plantId);
                if (plantToRemove != null)
                {
                    cart.Plants.Remove(plantToRemove);
                    var update = Builders<CartModel>.Update.Set(c => c.Plants, cart.Plants);
                    await _cartCollection.UpdateOneAsync(filter, update);
                }
            }
        }

        public static async Task ClearCartAsync(string UserId)
        {
            var filter = Builders<CartModel>.Filter.Eq(c => c.UserId, UserId);
            var update = Builders<CartModel>.Update.Set(c => c.Plants, new List<PlantModel>());
            await _cartCollection.UpdateOneAsync(filter, update);
        }


    }
}
