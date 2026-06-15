using WeatherForecast.Data;
using WeatherForecast.Models;

using Microsoft.EntityFrameworkCore;
using WeatherForecast.repository;
namespace WeatherForecast.Repositories;

public class SubscriptionRepository(AppDbContext db) : ISubscriptionRepository
{
    public async Task<Subscription?> GetByIdAsync(Guid id)
        => await db.Subscriptions
            .Include(s => s.Players)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<Subscription?> GetWithScheduleAsync(Guid id)
        => await db.Subscriptions
            .Include(s => s.Players)
            .Include(s => s.MatchDays.OrderBy(md => md.Date))
            .ThenInclude(md => md.Matches)
            .ThenInclude(m => m.Participants)
            .ThenInclude(mp => mp.Player)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<bool> ExistsSameAsync(Subscription subscription)
    {
        var candidates = await db.Subscriptions
            .Include(s => s.Players)
            .Where(s =>
                s.StartDate == subscription.StartDate &&
                s.EndDate == subscription.EndDate &&
                s.MatchDayOfWeek == subscription.MatchDayOfWeek &&
                s.SinglesMatchCount == subscription.SinglesMatchCount &&
                s.DoublesMatchCount == subscription.DoublesMatchCount)
            .ToListAsync();

        var incomingNames = subscription.Players
            .Select(p => p.Name)
            .OrderBy(x => x)
            .ToList();

        return candidates.Any(s =>
            s.Players
                .Select(p => p.Name)
                .OrderBy(x => x)
                .SequenceEqual(incomingNames));
    }

    public async Task AddAsync(Subscription subscription)
    {
        db.Subscriptions.Add(subscription);
        await db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Subscription subscription)
    {
        db.Subscriptions.Update(subscription);
        await db.SaveChangesAsync();
    }
}