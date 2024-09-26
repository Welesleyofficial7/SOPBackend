namespace SOPBackend.Services;

public interface IOrderItemService
{
    IEnumerable<OrderItem>? GetAllOrderItems();
    OrderItem? GetOrderItemById(Guid id);
    OrderItem? CreateOrderItem(OrderItem newOrderItem);
    OrderItem? UpdateOrderItem(Guid id, OrderItem updatedOrderItem);
    bool DeleteOrderItem(Guid id);
}