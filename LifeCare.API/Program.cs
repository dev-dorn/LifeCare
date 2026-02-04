using LifeCare.Application.Interfaces.Repositories;
using LifeCare.Infrastructure.Persistence;
using LifeCare.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(options =>
    {
        options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    }
    );
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Get connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=LifeCare.db";

// Add database context
builder.Services.AddDbContext<HospitalDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add repositories
builder.Services.AddScoped<IPatientRepository, PatientRepository>();

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("LifeCare.Application")));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configure Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LifeCare API v1");
    c.RoutePrefix = string.Empty; // Set Swagger UI at root
});

// Configure the pipeline
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Add default routes
app.MapGet("/", () => Results.Redirect("/swagger/index.html"));
app.MapGet("/api", () => Results.Redirect("/swagger/index.html"));
app.MapGet("/health", () => Results.Json(new 
{ 
    Status = "Healthy", 
    Timestamp = DateTime.UtcNow,
    Service = "LifeCare Hospital API",
    Version = "1.0",
    Environment = app.Environment.EnvironmentName
}));

// Initialize and seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<HospitalDbContext>();
        
        // Ensure database is created
        await context.Database.MigrateAsync();
        
        // Seed initial data
        await SeedDataAsync(context);
        
        Console.WriteLine("✅ Database initialized successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database initialization error: {ex.Message}");
    }
}

Console.WriteLine("======================================================");
Console.WriteLine("🚀 LifeCare Hospital API - .NET 10");
Console.WriteLine("📚 Activity 01: Identity & Intake");
Console.WriteLine("🌐 Endpoints:");
Console.WriteLine($"   • Swagger UI:    http://localhost:5000");
Console.WriteLine($"   • API:           http://localhost:5000/api/patients");
Console.WriteLine($"   • Health:        http://localhost:5000/health");
Console.WriteLine("======================================================");

app.Run();

// Seed data method
static async Task SeedDataAsync(HospitalDbContext context)
{
    if (!await context.Patients.AnyAsync())
    {
        Console.WriteLine("📊 Seeding initial patient data...");
        
        try
        {
            // Create patient 1
            var patient1 = LifeCare.Domain.Patients.Patient.CreateForSeed(
                id: Guid.NewGuid(),
                mrn: "LC-2024-0001",
                nationalId: "123-45-6789",
                firstName: "John",
                lastName: "Doe",
                dateOfBirth: new DateTime(1980, 5, 15),
                gender: "Male",
                phoneNumber: "+1-555-1234",
                email: "john.doe@example.com",
                street: "123 Main St",
                city: "Anytown",
                state: "CA",
                zipCode: "12345",
                createdAt: DateTime.UtcNow.AddDays(-1),
                createdBy: "System"
            );
            
            // Create patient 2
            var patient2 = LifeCare.Domain.Patients.Patient.CreateForSeed(
                id: Guid.NewGuid(),
                mrn: "LC-2024-0002",
                nationalId: "987-65-4321",
                firstName: "Jane",
                lastName: "Smith",
                dateOfBirth: new DateTime(1990, 8, 22),
                gender: "Female",
                phoneNumber: "+1-555-5678",
                email: "",
                street: "",
                city: "",
                state: "",
                zipCode: "",
                createdAt: DateTime.UtcNow.AddHours(-12),
                createdBy: "System"
            );
            
            // Create patient 3
            var patient3 = LifeCare.Domain.Patients.Patient.CreateForSeed(
                id: Guid.NewGuid(),
                mrn: "LC-2024-0003",
                nationalId: "456-78-9012",
                firstName: "Emma",
                lastName: "Johnson",
                dateOfBirth: new DateTime(2015, 3, 10),
                gender: "Female",
                phoneNumber: "+1-555-9012",
                email: "emma.parent@example.com",
                street: "789 Pine St",
                city: "Sometown",
                state: "TX",
                zipCode: "67890",
                createdAt: DateTime.UtcNow.AddHours(-6),
                createdBy: "System",
                guardianName: "Sarah Johnson",
                guardianRelationship: "Mother",
                guardianPhone: "+1-555-3456"
            );
            
            await context.Patients.AddAsync(patient1);
            await context.Patients.AddAsync(patient2);
            await context.Patients.AddAsync(patient3);
            
            await context.SaveChangesAsync();
            
            Console.WriteLine($"✅ Added 3 seed patients!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Seed error: {ex.Message}");
        }
    }
    else
    {
        var count = await context.Patients.CountAsync();
        Console.WriteLine($"✅ Database has {count} patients already");
    }
}