using FluentValidation;
using Moq;
using order.Services;
using order.Services.Interface;

namespace order.Tests;

public class UnitTest1
{
    private readonly OrderValidator _validator;

    public UnitTest1()
    {
        _validator = new OrderValidator();
    }
    
    
    [Fact]
    
    public async Task ProcessOrderAsync_ValidOrder_ReturnsSuccessResult()
    
    {
    
        // Arrange
    
        var mockCurrencyConverter = new Mock<ICurrencyConverter>();
    
        var mockAddressFormatter = new Mock<IAddressFormatter>();
    
        var orderService = new OrderService(mockCurrencyConverter.Object, mockAddressFormatter.Object);

        var order = new Order
        {
            Id = "A0000001",
            Name = "Test Order",
            Price = 100,
            Currency = "USD",
            Address = new Address { City = "Test City", District = "Test District", Street = "Test Street" }
        };

        mockCurrencyConverter.Setup(x => x.ConvertToTWDAsync(It.IsAny<decimal>(), It.IsAny<string>()))
            .ReturnsAsync(3000);
        mockAddressFormatter.Setup(x => x.Format(It.IsAny<Address>()))
            .Returns(new Address { City = "formatted city", District = "formatted district", Street = "formatted street" });

        // Act
        var result = await orderService.ProcessOrderAsync(order);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(3000, result.Data.Price);
        Assert.Equal("TWD", result.Data.Currency);
        Assert.Equal("formatted city", result.Data.Address.City);
    }
    
    [Fact]
    public void CreateOrder_NameContainsNonEnglishCharacters_ShouldReturnBadRequest()
    {
        var order = new Order { Id = "A0000001", Name = "測試Order", Address = new Address { City = "formatted city", District = "formatted district", Street = "formatted street" }, Price = 1000, Currency = "TWD" };
        var result = _validator.Validate(order);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage.Contains("Name must not contain non-English characters"));
    }

    [Fact]
    public void CreateOrder_NameIsNotCapitalized_ShouldReturnBadRequest()
    {
        var order = new Order { Id = "A0000001", Name = "test order", Address = new Address { City = "formatted city", District = "formatted district", Street = "formatted street" }, Price = 1000, Currency = "TWD" };
        var result = _validator.Validate(order);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage.Contains("Name must be capitalized"));
    }

    [Fact]
    public void CreateOrder_PriceIsOver2000_ShouldReturnBadRequest()
    {
        var order = new Order { Id = "A0000001", Name = "Test Order", Address = new Address { City = "formatted city", District = "formatted district", Street = "formatted street" }, Price = 2001, Currency = "TWD" };
        var result = _validator.Validate(order);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage.Contains("Price is over 2000"));
    }

    [Fact]
    public void CreateOrder_CurrencyIsNotTWDOrUSD_ShouldReturnBadRequest()
    {
        var order = new Order { Id = "A0000001", Name = "Test Order", Address = new Address { City = "formatted city", District = "formatted district", Street = "formatted street" }, Price = 1000, Currency = "EUR" };
        var result = _validator.Validate(order);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage.Contains("Currency format is wrong"));
    }
}

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(x => x.Name)
            .Must(BeEnglishOnly).WithMessage("Name must not contain non-English characters")
            .Must(BeCapitalized).WithMessage("Name must be capitalized");

        RuleFor(x => x.Price)
            .LessThanOrEqualTo(2000).WithMessage("Price is over 2000");

        RuleFor(x => x.Currency)
            .Must(BeValidCurrency).WithMessage("Currency format is wrong");
    }

    private bool BeEnglishOnly(string name)
    {
        return name.All(c => (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == ' ');
    }

    private bool BeCapitalized(string name)
    {
        return name.Split(' ').All(word => !string.IsNullOrEmpty(word) && char.IsUpper(word[0]));
    }

    private bool BeValidCurrency(string currency)
    {
        return currency == "TWD" || currency == "USD";
    }
}