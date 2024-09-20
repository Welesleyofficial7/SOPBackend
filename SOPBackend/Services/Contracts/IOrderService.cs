namespace SOPBackend.Services;

public interface IOrderService
{
    IEnumerable<Order>? GetAllOrders();
    Order? GetOrderById(Guid id);
    Order? CreateOrder(Order newUser);
    Order? UpdateOrder(Guid id, Order updatedUser);
    bool DeleteOrder(Guid id);
}