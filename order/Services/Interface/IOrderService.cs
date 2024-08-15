using order.Models;

public interface IOrderService
{
    Task<ApiResponse<Order>> ProcessOrderAsync(Order order);
}