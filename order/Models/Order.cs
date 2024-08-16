public class Order
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
}

public class Address
{
    public string City { get; set; }
    public string District { get; set; }
    public string Street { get; set; }

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(City) &&
               !string.IsNullOrWhiteSpace(District) &&
               !string.IsNullOrWhiteSpace(Street);
    }
}