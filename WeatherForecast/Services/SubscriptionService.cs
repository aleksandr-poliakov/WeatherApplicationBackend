using System.ComponentModel.DataAnnotations;
using WeatherForecast.Dto;
using WeatherForecast.Models;
using WeatherForecast.repository;
using WeatherForecast.Services;

public class SubscriptionService(ISubscriptionRepository repository, IScheduleGeneratorService generator) : ISubscriptionService
{
    public async Task<Guid> CreateAsync(CreateSubscriptionDto dto)
    {
        var subscription = MapToEntity(dto);

        if (await repository.ExistsSameAsync(subscription))
            throw new ValidationException("A schedule with the same parameters already exists.");

        subscription.MatchDays = generator.Generate(subscription);

        await repository.AddAsync(subscription);
        return subscription.Id;
    }

    public async Task<ScheduleDto> GetScheduleAsync(Guid id)
    {
        var subscription = await repository.GetWithScheduleAsync(id)
            ?? throw new ValidationException($"Subscription {id} not found.");

        return MapToScheduleDto(subscription);
    }

    public async Task<Guid> UpdateAsync(Guid id, UpdateSubscriptionDto dto)
    {
        var subscription = await repository.GetByIdAsync(id)
            ?? throw new ValidationException($"Subscription {id} not found.");

        subscription.StartDate = dto.StartDate;
        subscription.EndDate = dto.EndDate;
        subscription.MatchDayOfWeek = dto.MatchDayOfWeek;
        subscription.SinglesMatchCount = dto.SinglesMatchCount;
        subscription.DoublesMatchCount = dto.DoublesMatchCount;
        subscription.Players = dto.PlayerNames
            .Select(name => new Player
            {
                Id = Guid.NewGuid(),
                Name = name,
                SubscriptionId = subscription.Id
            }).ToList();

        subscription.MatchDays = generator.Generate(subscription);

        await repository.UpdateAsync(subscription);
        return subscription.Id;
    }

    private static Subscription MapToEntity(CreateSubscriptionDto dto) => new()
    {
        Id = Guid.NewGuid(),
        StartDate = dto.StartDate,
        EndDate = dto.EndDate,
        MatchDayOfWeek = dto.MatchDayOfWeek,
        SinglesMatchCount = dto.SinglesMatchCount,
        DoublesMatchCount = dto.DoublesMatchCount,
        CreatedAt = DateTime.UtcNow,
        Players = dto.PlayerNames
            .Select(name => new Player
            {
                Id = Guid.NewGuid(),
                Name = name
            }).ToList()
    };

    private static ScheduleDto MapToScheduleDto(Subscription subscription)
    {
        var allPlayers = subscription.Players.Select(p => p.Name).ToList();

        return new ScheduleDto
        {
            SubscriptionId = subscription.Id,
            MatchDays = subscription.MatchDays.Select(md =>
            {
                if (md.IsPlayFree)
                    return new MatchDayDto { Date = md.Date, IsPlayFree = true };

                var activePlayers = md.Matches
                    .SelectMany(m => m.Participants)
                    .Select(mp => mp.Player.Name)
                    .Distinct()
                    .ToList();

                return new MatchDayDto
                {
                    Date = md.Date,
                    IsPlayFree = false,
                    Spielfrei = allPlayers.Except(activePlayers).ToList(),
                    Singles = md.Matches
                        .Where(m => m.Type == MatchTypes.Singles)
                        .Select(m => new SinglesMatchDto
                        {
                            Player1 = m.Participants.ElementAt(0).Player.Name,
                            Player2 = m.Participants.ElementAt(1).Player.Name
                        }).ToList(),
                    Doubles = md.Matches
                        .Where(m => m.Type == MatchTypes.Doubles)
                        .Select(m => new DoublesMatchDto
                        {
                            Team1 = m.Participants.Take(2)
                                .Select(p => p.Player.Name).ToList(),
                            Team2 = m.Participants.Skip(2).Take(2)
                                .Select(p => p.Player.Name).ToList()
                        }).ToList()
                };
            }).ToList()
        };
    }
}