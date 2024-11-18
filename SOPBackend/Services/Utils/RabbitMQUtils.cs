using System.Data.Common;
using SOPBackend.Messages;
using Microsoft.AspNetCore.ResponseCaching;

namespace SOPBackend.Services.Utils;

public static class RabbitMQUtils
{
    public static NewUserMessage ToUserMessage(this User user, Guid id)
    {
        var message = new NewUserMessage()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            DestinationAddress = user.DestinationAddress,
            OrderId = id
        };
        return message;
    }
    
    public static NewOrderMessage ToMessage(this Order order)
    {
        var message = new NewOrderMessage()
        {
            Id = order.Id,
            PromotionId = order.PromotionId,
            TotalCost = order.TotalCost,
            OrderTime = order.OrderTime,
            Status = order.Status,
            UserId = order.UserId
        };
        return message;
    }
    
}

