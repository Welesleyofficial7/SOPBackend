using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MassTransit;
using SOPBackend.Utils;

namespace SOPBackend;
public class Order
{
    private Guid _id;
    private Guid _userId;
    private Status _status;
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
    [Column("userid")]
    public Guid UserId
    {
        get { return _userId; }
        set { _userId = value; }
    }

    [Required]
    [MaxLength(50)]
    [Column("status")]
    public Status Status
    {
        get { return _status; }
        set { _status = value; }
    }

    [Required]
    [Column("ordertime")]
    public DateTime OrderTime
    {
        get { return _orderTime; }
        set { _orderTime = value; }
    }

    [Required]
    [Column("totalcost")]
    public decimal TotalCost
    {
        get { return _totalCost; }
        set { _totalCost = value; }
    }
    
    [Column("promotionid")]
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

    public Order(Guid userId, Status status, DateTime orderTime, decimal totalCost, Guid? promotionId = null)
    {
        Id = NewId.NextGuid();
        UserId = userId;
        Status = status;
        OrderTime = orderTime;
        TotalCost = totalCost;
        PromotionId = promotionId;
    }

    private Order() { }
}
