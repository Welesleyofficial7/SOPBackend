using SOPBackend.KafkaMessages;

namespace SOPBackend.Services.Utils;

using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

public class KafkaProducer
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _topic;

    public KafkaProducer(IConfiguration configuration)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"]
        };
        _producer = new ProducerBuilder<Null, string>(config).Build();
        _topic = configuration["Kafka:Topic"];
    }

    public async Task ProduceAsync(Order newOrder)
    {
        var message = new OrderCreatedMessage
        {
            OrderId = newOrder.Id,
            UserId = newOrder.UserId,
            TotalCost = newOrder.TotalCost,
            Items = newOrder.OrderItems.Select(item => new OrderItemMessage
            {
                MenuItemId = item.MenuItemId,
                Quantity = item.Quantity,
                Subtotal = item.Subtotal
            }).ToList()
        };
        var messageValue = JsonSerializer.Serialize(message);
        await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = messageValue });
    }
    
}
