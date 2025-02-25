using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Bus;

public class TEventReader
{
    private readonly IDbContextFactory<TableDbContext> _context;

    public TEventReader(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    public async Task<bool> AssertAsync(Guid @event, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TEvent
            .AnyAsync(x => x.EventId == @event, token);
    }

    public async Task<TEventEntity?> FetchAsync(Guid @event, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TEvent
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.EventId == @event, token);
    }

    public async Task<int> CountAsync(IEventCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TEventEntity>> CollectAsync(IEventCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);
    }

    private IQueryable<TEventEntity> BuildQuery(IEventCriteria criteria)
    {
        using var db = _context.CreateDbContext();

        var query = db.TEvent.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.EventId != null)
        //    query = query.Where(x => x.EventId == criteria.EventId);

        // if (criteria.EventType != null)
        //    query = query.Where(x => x.EventType == criteria.EventType);

        // if (criteria.EventData != null)
        //    query = query.Where(x => x.EventData == criteria.EventData);

        // if (criteria.AggregateId != null)
        //    query = query.Where(x => x.AggregateId == criteria.AggregateId);

        // if (criteria.AggregateVersion != null)
        //    query = query.Where(x => x.AggregateVersion == criteria.AggregateVersion);

        // if (criteria.OriginId != null)
        //    query = query.Where(x => x.OriginId == criteria.OriginId);

        return query;
    }
}