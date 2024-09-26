namespace SOPBackend.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly ApplicationContext _context;

        public OrderItemService(ApplicationContext context)
        {
            _context = context;
        }
        
        public IEnumerable<OrderItem>? GetAllOrderItems()
        {
            return _context.OrderItems.ToList();
        }
        
        public OrderItem? GetOrderItemById(Guid id)
        {
            var orderItem = _context.OrderItems.Find(id);
            if (orderItem == null)
            {
                throw new Exception("Order item not found");
            }
            return orderItem;
        }
        
        public OrderItem? CreateOrderItem(OrderItem newOrderItem)
        {
            var check = _context.OrderItems.Find(newOrderItem.Id);

            if (check == null)
            {
                var entry = _context.OrderItems.Add(newOrderItem);
                _context.SaveChanges();
                return entry.Entity;
            }

            return null; 
        }
        
        public OrderItem? UpdateOrderItem(Guid id, OrderItem updatedOrderItem)
        {
            var orderItem = _context.OrderItems.Find(id);

            if (orderItem == null)
            {
                throw new Exception("Order item not found");
            }
            
            orderItem.Quantity = updatedOrderItem.Quantity;
            orderItem.Subtotal = updatedOrderItem.Subtotal;
            orderItem.MenuItemId = updatedOrderItem.MenuItemId;
            orderItem.OrderId = updatedOrderItem.OrderId;

            _context.OrderItems.Update(orderItem);
            _context.SaveChanges();

            return orderItem;
        }
        
        public bool DeleteOrderItem(Guid id)
        {
            var orderItem = _context.OrderItems.Find(id);

            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
                _context.SaveChanges();
                return true;
            }

            return false; 
        }
    }
}
