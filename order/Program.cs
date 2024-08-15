using order.Services;
using order.Services.Interface;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
.AddFluentValidation(fv => 
{
    fv.ImplicitlyValidateChildProperties = true;
    fv.ImplicitlyValidateRootCollectionElements = true;
    fv.RegisterValidatorsFromAssemblyContaining<OrderValidator>();
});


builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICurrencyConverter, CurrencyConverter>();
builder.Services.AddScoped<IAddressFormatter, AddressFormatter>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();