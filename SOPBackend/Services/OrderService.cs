using SOPBackend.DTOs;
using SOPBackend.Messages;
using SOPBackend.Utils;

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
        var entry = _context.Orders.Find(id) ?? null;
        _context.SaveChanges();
        return entry;
    }
    
    public Order? StartOrder(Guid id)
    {
        var entry = _context.Orders.Find(id) ?? null;
        if (entry == null)
        {
            throw new Exception("Order not found!");
        }
        entry.Status = Status.Pending;
        _context.Orders.Update(entry);
        _context.SaveChanges();

        return entry;
    }
    
    public Order? ApplyDiscountOrder(Guid id)
    {
        var entry = _context.Orders.Find(id) ?? null;
        if (entry == null)
        {
            throw new Exception("Order not found!");
        }
        entry.Status = Status.DiscountApplied;
        _context.Orders.Update(entry);
        _context.SaveChanges();

        return entry;
    }
    
    public Order? NotApplyDiscountOrder(Guid id)
    {
        var entry = _context.Orders.Find(id) ?? null;
        if (entry == null)
        {
            throw new Exception("Order not found!");
        }
        entry.Status = Status.DiscountNotApplied;
        _context.Orders.Update(entry);
        _context.SaveChanges();

        return entry;
    }

    public Order? CancelOrder(Guid id)
    {
        var entry = _context.Orders.Find(id) ?? null;
        if (entry == null)
        {
            throw new Exception("Order not found!");
        }
        entry.Status = Status.Canceled;
        _context.Orders.Update(entry);
        _context.SaveChanges();

        return entry;
    }

    public Order? StartPreparingOrder(Guid id)
    {
        var entry = _context.Orders.Find(id) ?? null;
        if (entry == null)
        {
            throw new Exception("Order not found!");
        }
        entry.Status = Status.BeingPrepared;
        _context.Orders.Update(entry);
        _context.SaveChanges();

        return entry;
    }

    public Order? CompleteOrder(Guid id)
    {
        var entry = _context.Orders.Find(id) ?? null;
        if (entry == null)
        {
            throw new Exception("Order not found!");
        }
        entry.Status = Status.Completed;
        _context.Orders.Update(entry);
        _context.SaveChanges();

        return entry;
    }
    public Order? CreateOrder(Order newUser)
    {
        var check = _context.Orders.Find(newUser.Id);

        if (check == null)
        {
            var entry = _context.Orders.Add(newUser);
            _context.SaveChanges();
            return entry.Entity;
        }

        return null;
    }

    public Order? UpdateOrder(Guid id, Order updatedUser)
    {
        var check = _context.Orders.Find(id);
        
        if (check == null)
        {
            throw new Exception("Order not found!");
        }

        check.Status = updatedUser.Status;
        check.TotalCost = updatedUser.TotalCost;
        check.OrderTime = updatedUser.OrderTime;
        check.PromotionId = updatedUser.PromotionId;
        check.OrderItems = updatedUser.OrderItems;

        _context.Orders.Update(check);
        _context.SaveChanges();

        return check;
    }

    public bool DeleteOrder(Guid id)
    {
        var check = _context.Orders.Find(id);

        if (check != null)
        {
            _context.Orders.Remove(check);
            _context.SaveChanges();
            return true;
        }

        return false;
    }
}