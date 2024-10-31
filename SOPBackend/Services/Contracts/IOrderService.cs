using SOPBackend.DTOs;

namespace SOPBackend.Services;

public interface IOrderService
{
    IEnumerable<Order>? GetAllOrders();
    Order? GetOrderById(Guid id);
    Order? CreateOrder(Order newOrder);
    Order? UpdateOrder(Guid id, Order updatedOrder);
    bool DeleteOrder(Guid id);
    Order? CancelOrder(Guid id);
    Order? StartPreparingOrder(Guid id);
    Order? CompleteOrder(Guid id);
    Order? ApplyDiscountOrder(Guid id);

}