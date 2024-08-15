using order.Services.Interface;

namespace order.Services;

public class CurrencyConverter : ICurrencyConverter
{
    public Task<decimal> ConvertToTWDAsync(decimal amount, string fromCurrency)
    {
        var rate = fromCurrency switch
        {
            "USD" => 31,
            "TWD" => 1,
            _ => 0 // 如果是未知貨幣，返回0表示轉換失敗
        };
        return Task.FromResult(amount * rate);
    }
}