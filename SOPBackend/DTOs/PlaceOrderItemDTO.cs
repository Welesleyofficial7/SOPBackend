namespace SOPBackend.DTOs;

public class PlaceOrderItemDTO
{
    public Guid MenuItemId { get; set; }
    public int Quantity { get; set; }
    public int Subtotal { get; set; }
}