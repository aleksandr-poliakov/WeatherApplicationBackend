using WeatherForecast.Dto;

namespace WeatherForecast.Services;

public interface ISubscriptionService
{
    Task<Guid> CreateAsync(CreateSubscriptionDto dto);
    Task<ScheduleDto> GetScheduleAsync(Guid id);
    Task<Guid> UpdateAsync(Guid id, UpdateSubscriptionDto dto);
}