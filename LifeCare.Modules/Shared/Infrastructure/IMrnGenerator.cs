// LifeCare.Infrastructure/Services/IMrnGenerator.cs
namespace LifeCare.Modules.Shared.Infrastructure
{
    public interface IMrnGenerator
    {
        Task<string> GenerateAsync();
    }
}