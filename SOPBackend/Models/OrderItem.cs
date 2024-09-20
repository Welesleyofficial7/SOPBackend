using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOPBackend;

public class OrderItem
{
    private Guid _id;
    private Guid _orderId;
    private Guid _menuItemId;
    private int _quantity;
    private decimal _subtotal;

    [Key]
    public Guid Id
    {
        get { return _id; }
        private set { _id = value; }
    }

    [Required]
    public Guid OrderId
    {
        get { return _orderId; }
        set { _orderId = value; }
    }

    [Required]
    public Guid MenuItemId
    {
        get { return _menuItemId; }
        set { _menuItemId = value; }
    }

    [Required]
    private int Quantity
    {
        get { return _quantity; }
        set { _quantity = value; }
    }

    [Required]
    private decimal Subtotal
    {
        get { return _subtotal; }
        set { _subtotal = value; }
    }

    [ForeignKey("OrderId")]
    public virtual Order Order { get; set; }

    [ForeignKey("MenuItemId")]
    public virtual MenuItem MenuItem { get; set; }

    public OrderItem(Guid orderId, Guid menuItemId, int quantity, decimal subtotal)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        MenuItemId = menuItemId;
        Quantity = quantity;
        Subtotal = subtotal;
    }

    private OrderItem() { }
}