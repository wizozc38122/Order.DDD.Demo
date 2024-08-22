using Microsoft.EntityFrameworkCore;
using Order.DDD.Demo.Adapter.Out;
using Order.DDD.Demo.Adapter.Out.Implementation;
using Order.DDD.Demo.UseCase;
using Order.DDD.Demo.UseCase.Port.In;
using Order.DDD.Demo.UseCase.Port.Out;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// UseCase
builder.Services.AddScoped<ICreateOrderService, CreateOrderService>();
builder.Services.AddScoped<IConfirmPaymentService, ConfirmPaymentService>();
builder.Services.AddScoped<IPickOrderItemsService, PickOrderItemsService>();
builder.Services.AddScoped<ICompleteOrderService, CompleteOrderService>();
builder.Services.AddScoped<ICancelOrderService, CancelOrderService>();
builder.Services.AddSingleton(TimeProvider.System);

// OutPort
builder.Services.AddScoped<IOrderOutPort, OrderRepository>();

// DbContext
var connectionString = Environment.GetEnvironmentVariable("ConnectionString")
                       ?? throw new ArgumentNullException(nameof(Program), "ConnectionString is null");
builder.Services.AddDbContext<OrderDbContext>(
    o =>
        o.UseSqlServer(connectionString));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Demo DB Migration
var needMigrate = Environment.GetEnvironmentVariable("NeedMigrate") ?? "N";
if (needMigrate == "Y")
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<OrderDbContext>();
    if (!context.Database.CanConnect())
    {
        context.Database.EnsureCreated();
    }
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();