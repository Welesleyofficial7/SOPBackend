using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOPBackend;
public class Order
{
    private Guid _id;
    private Guid _userId;
    private string _status;
    private DateTime _orderTime;
    private decimal _totalCost;
    private Guid? _promotionId;

    [Key]
    public Guid Id
    {
        get { return _id; }
        private set { _id = value; }
    }

    [Required]
    public Guid UserId
    {
        get { return _userId; }
        set { _userId = value; }
    }

    [Required]
    [MaxLength(50)]
    private string Status
    {
        get { return _status; }
        set { _status = value; }
    }

    [Required]
    private DateTime OrderTime
    {
        get { return _orderTime; }
        set { _orderTime = value; }
    }

    [Required]
    private decimal TotalCost
    {
        get { return _totalCost; }
        set { _totalCost = value; }
    }
    
    public Guid? PromotionId
    {
        get { return _promotionId; }
        set { _promotionId = value; }
    }
    
    [ForeignKey("PromotionId")]
    public virtual Promotion Promotion { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; }

    public Order(Guid userId, string status, DateTime orderTime, decimal totalCost, Guid? promotionId = null)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Status = status;
        OrderTime = orderTime;
        TotalCost = totalCost;
        PromotionId = promotionId;
    }

    private Order() { }
}
