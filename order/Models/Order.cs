public class Order
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }

    public Order(string id, string name, Address address, decimal price, string currency)
    {
        Id = id;
        Name = name;
        Address = address;
        Price = price;
        Currency = currency;
    }
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