using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MassTransit;
using SOPBackend.Messages;

namespace SOPBackend;

public class MenuItem
{
    private Guid _id;
    private string _name;
    private string _description;
    private decimal _price;
    private Category _category;

    [Key]
    public Guid Id
    {
        get { return _id; }
        private set { _id = value; }
    }

    [Required]
    [MaxLength(36)]
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    [MaxLength(512)]
    public string Description
    {
        get { return _description; }
        set { _description = value; }
    }

    [Required]
    public decimal Price
    {
        get { return _price; }
        set { _price = value; }
    }

    [Required]
    [MaxLength(50)]
    public Category Category
    {
        get { return _category; }
        set { _category = value; }
    }

    public MenuItem(string name, string description, decimal price, Category category)
    {
        Id = NewId.NextGuid();
        Name = name;
        Description = description;
        Price = price;
        Category = category;
    }

    private MenuItem() { }
}