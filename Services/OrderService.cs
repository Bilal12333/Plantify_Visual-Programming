using DAL;
using Entities;
using MongoDB.Driver;

namespace PlantifyAdmin.Services
{
    public class OrderService : IOrderService
    {
        public async Task SaveOrderAsync(OrderModel order)
        {
            await MongoHelper.GetOrderCollection.InsertOneAsync(order);
        }
    }
}
