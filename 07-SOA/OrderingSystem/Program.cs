using Microsoft.EntityFrameworkCore;
using OrderingSystem.DataAccess;
using OrderingSystem.DataAccess.Repositories;
using OrderingSystem.BusinessLogic.Interfaces;
using OrderingSystem.BusinessLogic.Services;
using OrderingSystem.DataValidation.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Register Business BusinessLogic
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
// builder.Services.AddScoped<IPaymentService, PaymentService>(); // REMOVED: SOA migration

// Register Validation Services
builder.Services.AddScoped<IOrderValidationService, OrderValidationService>();
builder.Services.AddScoped<IProductValidationService, ProductValidationService>();
builder.Services.AddScoped<IPaymentValidationService, PaymentValidationService>();

// Register SOA Clients
builder.Services.AddScoped<OrderingSystem.BusinessLogic.Integration.Interfaces.IEnterprisePaymentClient, OrderingSystem.BusinessLogic.Integration.Clients.EnterprisePaymentClient>();
builder.Services.AddScoped<OrderingSystem.BusinessLogic.Integration.Interfaces.IEnterpriseNotificationClient, OrderingSystem.BusinessLogic.Integration.Clients.EnterpriseNotificationClient>();

// Register Micro-Kernel Components
builder.Services.AddScoped<OrderingSystem.BusinessLogic.MicroKernel.Interfaces.IOrderProcessingCore, OrderingSystem.BusinessLogic.MicroKernel.Core.OrderProcessingCore>();

// Plugins (Order determines execution order)
builder.Services.AddScoped<OrderingSystem.BusinessLogic.MicroKernel.Interfaces.IOrderPlugin, OrderingSystem.BusinessLogic.MicroKernel.Plugins.DiscountPlugin>();
builder.Services.AddScoped<OrderingSystem.BusinessLogic.MicroKernel.Interfaces.IOrderPlugin, OrderingSystem.BusinessLogic.MicroKernel.Plugins.TaxPlugin>();
// builder.Services.AddScoped<OrderingSystem.BusinessLogic.MicroKernel.Interfaces.IOrderPlugin, OrderingSystem.BusinessLogic.MicroKernel.Plugins.PaymentPlugin>(); // REMOVED: SOA Migration

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
