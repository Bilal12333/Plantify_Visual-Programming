using Entities;

namespace PlantifyAdmin.Services
{
    public interface IOrderService
    {
        Task SaveOrderAsync(OrderModel order);
    }
}
