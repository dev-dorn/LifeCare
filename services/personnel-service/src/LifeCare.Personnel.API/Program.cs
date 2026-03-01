using LifeCare.Personnel.Application.Interfaces;
using LifeCare.Personnel.Infrastructure.Caching;
using LifeCare.Personnel.Infrastructure.Messaging;
using LifeCare.Personnel.Infrastructure.Persistence;
using LifeCare.Personnel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Mediatr
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(LifeCare.Personnel.Application.Commands.RegisterPersonnelCommand).Assembly));
// database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=lifecare-personnel-db;Port=5432;Database=personnel_db;Username=Admin;Password=Admin123";

builder.Services.AddDbContext<PersonnelDbContext>(options =>
    options.UseNpgsql(connectionString));  

var redisConnection = builder.Configuration["Redis:ConnectionString"] ?? "lifecare-redis:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(redisConnection));

builder.Services.AddScoped<ICacheServices, RedisCacheService>();

//RabbitMq
var rabbitHost = builder.Configuration["RabbitMq:Host"] ?? "lifecare-rabbitmq";
var rabbitUser = builder.Configuration["RabbitMq:Username"] ?? "lifecare";
var rabbitPass = builder.Configuration["RabbitMq:Password"] ?? "life_care";

builder.Services.AddSingleton<IEventBus>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<RabbitMqEventBus>>();
    return new RabbitMqEventBus(rabbitHost, rabbitUser, rabbitPass
        , logger);
    
});
builder.Services.AddScoped<IPersonnelRepository, PersonnelRepository>();

//Cors

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthorization();
app.MapControllers();

// Database migration on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PersonnelDbContext>();
    try
    {
        context.Database.Migrate();
        Console.WriteLine("✅ Database migrated successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database migration error: {ex.Message}");
    }
}

Console.WriteLine(@"
======================================================
🚀 LifeCare Personnel Service
📋 Manage hospital staff and personnel
🌐 Endpoints:
   • Swagger UI:    http://localhost:8081
   • API:           http://localhost:8081/api/personnel
======================================================
");

app.Run();

