namespace SOPBackend.Services;

public interface IOrderService
{
    IEnumerable<Order>? GetAllOrders();
    Order? GetOrderById(Guid id);
    Order? CreateOrder(Order newOrder);
    Order? UpdateOrder(Guid id, Order updatedOrder);
    bool DeleteOrder(Guid id);
}