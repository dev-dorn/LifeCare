// LifeCare.Infrastructure/Services/IMrnGenerator.cs
namespace LifeCare.Infrastructure.Services
{
    public interface IMrnGenerator
    {
        Task<string> GenerateAsync();
    }
}