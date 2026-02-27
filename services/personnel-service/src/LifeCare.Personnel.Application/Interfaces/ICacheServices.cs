namespace LifeCare.Personnel.Application.Interfaces;

public interface ICacheServices
{
    Task<T> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
    Task RemoveAsync(string key);
    Task<bool> ExistingAsync(string key);
}