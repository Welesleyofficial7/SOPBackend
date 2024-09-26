using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SOPBackend;

public class PromotionDTO
{
    [Required]
    [MaxLength(50)]
    public string Code { get; set; }

    [Required]
    [Range(0, 100)]
    public decimal Discount { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public PromotionDTO(string code, decimal discount, DateTime startDate, DateTime endDate)
    {
        Code = code;
        Discount = discount;
        StartDate = startDate;
        EndDate = endDate;
    }

}