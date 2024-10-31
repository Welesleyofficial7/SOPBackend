using SOPBackend.Messages;
using SOPBackend.Utils;

namespace SOPBackend.DTOs;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class GetAllOrdersDTO
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    public Guid UserId { get; set; }

    [Required]
    [MaxLength(50)]
    public Status Status { get; set; }

    [Required]
    public DateTime OrderTime { get; set; }

    [Required]
    public decimal TotalCost { get; set; }

    public GetAllOrdersDTO(Guid userId, Status status, DateTime orderTime, decimal totalCost)
    {
        UserId = userId;
        Status = status;
        OrderTime = orderTime;
        TotalCost = totalCost;
    }
}