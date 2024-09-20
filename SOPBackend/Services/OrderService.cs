namespace SOPBackend.Services;

public class OrderService : IOrderService
{
    private ApplicationContext _context;
    
    public OrderService(ApplicationContext context)
    {
        _context = context;
    }
    
    public IEnumerable<Order>? GetAllOrders()
    {
        return _context.Orders.ToList();
    }

    public Order? GetOrderById(Guid id)
    {
        return _context.Orders.Find(id) ?? null;
    }

    public Order? CreateOrder(Order newUser)
    {
        var check = _context.Orders.Find(newUser.Id);

        if (check == null)
        {
            var entry = _context.Orders.Add(newUser);
            return entry.Entity;
        }

        return null;
    }

    public Order? UpdateOrder(Guid id, Order updatedUser)
    {
        var check = _context.Orders.Find(id);
        if (check != null)
        {
            _context.Orders.Update(updatedUser);
        }

        return null;
    }

    public bool DeleteOrder(Guid id)
    {
        throw new NotImplementedException();
    }
}