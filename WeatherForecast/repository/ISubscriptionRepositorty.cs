using WeatherForecast.Models;

namespace WeatherForecast.repository;

public interface ISubscriptionRepository
{
    Task<Subscription?> GetByIdAsync(Guid id);
    Task<Subscription?> GetWithScheduleAsync(Guid id);
    Task<bool> ExistsSameAsync(Subscription subscription);
    Task AddAsync(Subscription subscription);
    Task UpdateAsync(Subscription subscription);
}