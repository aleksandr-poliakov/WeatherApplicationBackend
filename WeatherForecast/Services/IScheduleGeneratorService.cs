
using WeatherForecast.Models;

namespace WeatherForecast.Services;

public interface IScheduleGeneratorService
{
    List<MatchDay> Generate(Subscription subscription);
}