using WeatherForecast.Dto;
using WeatherForecast.Models;

namespace WeatherForecast.Services;

public interface IScheduleGeneratorService
{
    ScheduleDto Generate(CreateSubscriptionDto input);
}