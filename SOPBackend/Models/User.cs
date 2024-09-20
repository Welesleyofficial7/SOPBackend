using System.ComponentModel.DataAnnotations;

namespace SOPBackend;

public class User
{
    private Guid _id;
    private string _name;
    private string _email;
    private string _phoneNumber;
    private string? _destinationAddress;
    
    [Key]
    public Guid Id
    {
        get { return _id; }
        set { _id = value; }
    }
    
    [Required]
    [MaxLength(36)]
    public string Name
    {
        get { return _name; }
        set { _name = value;  }
    }

    [Required]
    [EmailAddress]
    public string Email
    {
        get { return _email;  }
        set { _email = value;  }
    }

    [Phone]
    public string PhoneNumber
    {
        get { return _phoneNumber; }
        set { _phoneNumber = value; }
    }

    [MaxLength(256)]
    public string DestinationAddress
    {
        get { return _destinationAddress??""; }
        set
        {
            _destinationAddress = value;
        }
    }
    
    public virtual ICollection<Order> Orders { get; set; }

    public User(string Name, string Email, string PhoneNumber, string DestinationAddress)
    {
        Id = Guid.NewGuid();
        this.Name = Name;
        this.Email = Email;
        this.PhoneNumber = PhoneNumber;
        this.DestinationAddress = DestinationAddress;
    }

    private User()
    {
    }
}


