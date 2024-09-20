using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOPBackend;

public class MenuItem
{
    private Guid _id;
    private string _name;
    private string _description;
    private decimal _price;
    private string _category;

    [Key]
    public Guid Id
    {
        get { return _id; }
        private set { _id = value; }
    }

    [Required]
    [MaxLength(36)]
    private string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    [MaxLength(512)]
    private string Description
    {
        get { return _description; }
        set { _description = value; }
    }

    [Required]
    private decimal Price
    {
        get { return _price; }
        set { _price = value; }
    }

    [Required]
    [MaxLength(50)]
    private string Category
    {
        get { return _category; }
        set { _category = value; }
    }

    public virtual ICollection<OrderItem> OrderItems { get; set; }

    public MenuItem(string name, string description, decimal price, string category)
    {
        Id = Guid.NewGuid();
        this.Name = name;
        this.Description = description;
        this.Price = price;
        this.Category = category;
    }

    private MenuItem() { }
}