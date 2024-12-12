using Microsoft.AspNetCore.SignalR;

namespace SOPBackend.Hubs;

public class OrderHub : Hub
{
    public async Task SendOrderUpdate(string orderId, string status)
    {
        Console.WriteLine($"Обновление статуса заказа - OrderId: {orderId}, Status: {status}");
        await Clients.All.SendAsync("ReceiveOrderUpdate", new { OrderId = orderId, Status = status});
    }

    public async Task JoinOrderGroup(string orderId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, orderId);
    }

    public async Task LeaveOrderGroup(string orderId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, orderId);
    }
}
