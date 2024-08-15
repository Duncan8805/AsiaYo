using order.Models;
using order.Services.Interface;

namespace order.Services;

public class OrderService : IOrderService
{
    private readonly ICurrencyConverter _currencyConverter;
    private readonly IAddressFormatter _addressFormatter;

    public OrderService(ICurrencyConverter currencyConverter, IAddressFormatter addressFormatter)
    {
        _currencyConverter = currencyConverter;
        _addressFormatter = addressFormatter;
    }

    public async Task<ApiResponse<Order>> ProcessOrderAsync(Order order)
    {
        try
        {
            // 轉換幣別
            var convertedPrice = await _currencyConverter.ConvertToTWDAsync(order.Price, order.Currency);
            if (convertedPrice == 0)
            {
                return new ApiResponse<Order>
                {
                    Success = false,
                    Message = "Currency conversion failed",
                    Data = null
                };
            }
            order.Price = convertedPrice;
            order.Currency = "TWD";

            // 轉換地址格式
            var formattedAddress = _addressFormatter.Format(order.Address);
            if (formattedAddress == null)
            {
                return new ApiResponse<Order>
                {
                    Success = false,
                    Message = "Address formatting failed",
                    Data = null
                };
            }
            order.Address = formattedAddress;

            return new ApiResponse<Order>
            {
                Success = true,
                Message = "Order processed successfully",
                Data = order
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Order>
            {
                Success = false,
                Message = $"An error occurred while processing the order: {ex.Message}",
                Data = null
            };
        }
    }

}