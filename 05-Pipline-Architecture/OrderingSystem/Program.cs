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
builder.Services.AddScoped<IPaymentService, PaymentService>();

// Register Validation Services
builder.Services.AddScoped<IOrderValidationService, OrderValidationService>();
builder.Services.AddScoped<IProductValidationService, ProductValidationService>();
builder.Services.AddScoped<IPaymentValidationService, PaymentValidationService>();

// Register Pipeline Components
builder.Services.AddScoped<OrderingSystem.BusinessLogic.Pipeline.OrderCreation.Steps.ValidationStep>();
builder.Services.AddScoped<OrderingSystem.BusinessLogic.Pipeline.OrderCreation.Steps.OrderInitializationStep>();
builder.Services.AddScoped<OrderingSystem.BusinessLogic.Pipeline.OrderCreation.Steps.PaymentStep>();
builder.Services.AddScoped<OrderingSystem.BusinessLogic.Pipeline.OrderCreation.Steps.OrderCompletionStep>();
builder.Services.AddScoped<OrderingSystem.BusinessLogic.Pipeline.OrderCreation.IOrderCreationPipeline, OrderingSystem.BusinessLogic.Pipeline.OrderCreation.OrderCreationPipeline>();

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
