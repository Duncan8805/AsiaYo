using order.Services.Interface;

namespace order.Services;

public class AddressFormatter : IAddressFormatter
{
    public Address Format(Address address)
    {
        if (address == null || !address.IsValid())
        {
            return null;
        }

        // 這裡實現實際的地址格式轉換邏輯
        return new Address
        {
            City = address.City.ToLower().Replace(" ", "-"),
            District = address.District.ToLower().Replace(" ", "-"),
            Street = address.Street.ToLower().Replace(" ", "-")
        };
    }
}