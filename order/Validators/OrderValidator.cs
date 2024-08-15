using FluentValidation;
using System.Text.RegularExpressions;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Matches(@"^[A-Z]\d{7}$")
            .WithMessage("ID must be in the format A0000000");

        RuleFor(x => x.Name)
            .NotEmpty()
            .Must(BeValidName)
            .WithMessage("Name must not contain non-English characters and must be capitalized");

        RuleFor(x => x.Price)
            .Must(BeValidPrice)
            .WithMessage("Price is over 2000");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Must(BeValidCurrency)
            .WithMessage("Currency format is wrong");

        RuleFor(x => x.Address).SetValidator(new AddressValidator());
    }

    private bool BeValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        // 檢查是否只包含英文字符
        if (!Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
            return false;

        // 檢查每個單詞是否首字母大寫
        var words = name.Split(' ');
        return words.All(word => char.IsUpper(word[0]) && word.Substring(1).All(char.IsLower));
    }

    private bool BeValidPrice(decimal price)
    {
        return price <= 2000;
    }

    private bool BeValidCurrency(string currency)
    {
        return new[] { "TWD", "USD" }.Contains(currency);
    }
}

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(x => x.City).NotEmpty().MaximumLength(50);
        RuleFor(x => x.District).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Street).NotEmpty().MaximumLength(100);
    }
}