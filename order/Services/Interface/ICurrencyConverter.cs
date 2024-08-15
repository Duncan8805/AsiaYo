namespace order.Services.Interface;

public interface ICurrencyConverter
{
    Task<decimal> ConvertToTWDAsync(decimal amount, string fromCurrency);
}