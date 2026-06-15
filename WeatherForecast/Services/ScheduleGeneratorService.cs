using WeatherForecast.Exceptions;
using WeatherForecast.Models;

namespace WeatherForecast.Services;

public class ScheduleGeneratorService : IScheduleGeneratorService
{
    public List<MatchDay> Generate(Subscription subscription)
    {
        Validate(subscription);

        var players = subscription.Players.ToList();
        var matchDates = GetMatchDates(
            subscription.StartDate,
            subscription.EndDate,
            subscription.MatchDayOfWeek);

        var result = new List<MatchDay>();
        int rotation = 0;
        int activeCount = GetActivePlayerCount(
            subscription.SinglesMatchCount,
            subscription.DoublesMatchCount);

        foreach (var date in matchDates)
        {
            var active = PickPlayers(players, rotation, activeCount);
            rotation++;

            var matchDay = new MatchDay
            {
                Id = Guid.NewGuid(),
                SubscriptionId = subscription.Id,
                Date = date,
                IsPlayFree = false,
                Matches = BuildMatches(
                    active,
                    subscription.SinglesMatchCount,
                    subscription.DoublesMatchCount)
            };

            result.Add(matchDay);
        }

        return result;
    }

    private static List<Match> BuildMatches(
        List<Player> active,
        int singlesCount,
        int doublesCount)
    {
        var matches = new List<Match>();

        for (int i = 0; i < singlesCount; i++)
        {
            var match = new Match
            {
                Id = Guid.NewGuid(),
                Type = MatchTypes.Singles,
                Participants =
                [
                    new MatchParticipant { PlayerId = active[i * 2].Id },
                    new MatchParticipant { PlayerId = active[i * 2 + 1].Id }
                ]
            };
            foreach (var p in match.Participants)
                p.MatchId = match.Id;

            matches.Add(match);
        }

        for (int i = 0; i < doublesCount; i++)
        {
            var participants = singlesCount > 0
                ? new List<MatchParticipant>
                {
                    new() { PlayerId = active[0].Id },
                    new() { PlayerId = active[1].Id },
                    new() { PlayerId = active[2].Id },
                    new() { PlayerId = active[3].Id }
                }
                : active.Select(p => new MatchParticipant { PlayerId = p.Id }).ToList();

            var match = new Match
            {
                Id = Guid.NewGuid(),
                Type = MatchTypes.Doubles,
                Participants = participants
            };
            foreach (var p in match.Participants)
                p.MatchId = match.Id;

            matches.Add(match);
        }

        return matches;
    }

    private static void Validate(Subscription subscription)
    {
        if (subscription.Players == null || subscription.Players.Count < 2)
            throw new ValidationException("At least 2 players required.");

        if (subscription.SinglesMatchCount == 0 && subscription.DoublesMatchCount == 0)
            throw new ValidationException("At least one singles or doubles match required.");

        if (subscription.EndDate <= subscription.StartDate)
            throw new ValidationException("End date must be after start date.");

        if (subscription.Players.Any(p => string.IsNullOrWhiteSpace(p.Name)))
            throw new ValidationException("Player names cannot be empty.");

        if (subscription.Players.Select(p => p.Name).Distinct().Count()
            != subscription.Players.Count)
            throw new ValidationException("Player names must be unique.");

        int activeNeeded = GetActivePlayerCount(
            subscription.SinglesMatchCount,
            subscription.DoublesMatchCount);

        if (subscription.Players.Count < activeNeeded)
            throw new ValidationException(
                $"Need at least {activeNeeded} players for this configuration.");
    }

    private static int GetActivePlayerCount(int singlesCount, int doublesCount)
    {
        if (singlesCount > 0) return singlesCount * 2;
        return doublesCount * 4;
    }

    private static List<DateOnly> GetMatchDates(DateOnly start, DateOnly end, DayOfWeek day)
    {
        var dates = new List<DateOnly>();
        var current = start;
        while (current <= end)
        {
            if (current.DayOfWeek == day)
                dates.Add(current);
            current = current.AddDays(1);
        }
        return dates;
    }

    private static List<Player> PickPlayers(List<Player> players, int rotation, int count)
    {
        var result = new List<Player>();
        for (int i = 0; i < count; i++)
            result.Add(players[(rotation + i) % players.Count]);
        return result;
    }
}