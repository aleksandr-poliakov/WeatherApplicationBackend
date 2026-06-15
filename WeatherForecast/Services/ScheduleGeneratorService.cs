using WeatherForecast.Dto;
using WeatherForecast.Exception;

namespace WeatherForecast.Services;

public class ScheduleGeneratorService : IScheduleGeneratorService
{
    public ScheduleDto Generate(CreateSubscriptionDto input)
    {
        Validate(input);

        var players = input.PlayerNames;
        var matchDates = GetMatchDates(input.StartDate, input.EndDate, input.MatchDayOfWeek);
        var result = new ScheduleDto();
        int rotation = 0;
        int activeCount = GetActivePlayerCount(input.SinglesMatchCount, input.DoublesMatchCount);

        foreach (var date in matchDates)
        {
            var active = PickPlayers(players, rotation, activeCount);
            var spielfrei = players.Except(active).ToList();
            rotation++;

            var dayDto = new MatchDayDto
            {
                Date = date,
                Spielfrei = spielfrei
            };

            if (input.SinglesMatchCount > 0)
            {
                for (int i = 0; i < input.SinglesMatchCount; i++)
                {
                    dayDto.Singles.Add(new SinglesMatchDto
                    {
                        Player1 = active[i * 2],
                        Player2 = active[i * 2 + 1]
                    });
                }
            }

            if (input.DoublesMatchCount > 0)
            {
                for (int i = 0; i < input.DoublesMatchCount; i++)
                {
                    if (input.SinglesMatchCount > 0)
                    {
                        dayDto.Doubles.Add(new DoublesMatchDto
                        {
                            Team1 = [active[0], active[1]],
                            Team2 = [active[2], active[3]]
                        });
                    }
                    else
                    {
                        int offset = i * 4;
                        dayDto.Doubles.Add(new DoublesMatchDto
                        {
                            Team1 = [active[offset], active[offset + 1]],
                            Team2 = [active[offset + 2], active[offset + 3]]
                        });
                    }
                }
            }

            result.MatchDays.Add(dayDto);
        }

        return result;
    }

    private static void Validate(CreateSubscriptionDto input)
    {
        if (input.PlayerNames == null || input.PlayerNames.Count < 2)
            throw new ValidationException("At least 2 players required.");

        if (input.SinglesMatchCount == 0 && input.DoublesMatchCount == 0)
            throw new ValidationException("At least one singles or doubles match required.");

        if (input.EndDate <= input.StartDate)
            throw new ValidationException("End date must be after start date.");

        if (input.SinglesMatchCount < 0 || input.DoublesMatchCount < 0)
            throw new ValidationException("Match counts cannot be negative.");

        int activeNeeded = GetActivePlayerCount(input.SinglesMatchCount, input.DoublesMatchCount);
        if (input.PlayerNames.Count < activeNeeded)
            throw new ValidationException($"Need at least {activeNeeded} players for this configuration.");

        if (input.PlayerNames.Distinct().Count() != input.PlayerNames.Count)
            throw new ValidationException("Player names must be unique.");

        if (input.PlayerNames.Any(string.IsNullOrWhiteSpace))
            throw new ValidationException("Player names cannot be empty.");
    }

    private static int GetActivePlayerCount(int singlesCount, int doublesCount)
    {
        if (singlesCount > 0 && doublesCount > 0)
            return singlesCount * 2;
        if (singlesCount > 0)
            return singlesCount * 2;
        if (doublesCount > 0)
            return doublesCount * 4;
        return 0;
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

    private static List<string> PickPlayers(List<string> players, int rotation, int count)
    {
        var result = new List<string>();
        for (int i = 0; i < count; i++)
            result.Add(players[(rotation + i) % players.Count]);
        return result;
    }
}