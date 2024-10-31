using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MassTransit;
using Newtonsoft.Json;

namespace SOPBackend;

public class Promotion
{
    private Guid _id;
    private string _code;
    private decimal _discount;
    private DateTime _startDate;
    private DateTime _endDate;

    [Key]
    public Guid Id
    {
        get { return _id; }
        private set { _id = value; }
    }

    [Required]
    [MaxLength(50)]
    public string Code
    {
        get { return _code; }
        set { _code = value; }
    }

    [Required]
    [Range(0, 100)]
    public decimal Discount
    {
        get { return _discount; }
        set { _discount = value; }
    }

    [Required]
    public DateTime StartDate
    {
        get { return _startDate; }
        set { _startDate = value; }
    }

    [Required]
    public DateTime EndDate
    {
        get { return _endDate; }
        set { _endDate = value; }
    }
    
    [JsonIgnore]
    public virtual ICollection<Order> Orders { get; set; }

    public Promotion(string code, decimal discount, DateTime startDate, DateTime endDate)
    {
        Id = NewId.NextGuid();
        Code = code;
        Discount = discount;
        StartDate = startDate;
        EndDate = endDate;
    }

    private Promotion() {}
}