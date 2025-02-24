using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Bus;

internal class TAggregateReader
{
    private readonly IDbContextFactory<TableDbContext> _context;

    internal TAggregateReader(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    internal async Task<bool> AssertAsync(Guid aggregate, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TAggregate
            .AnyAsync(x => x.AggregateId == aggregate, token);
    }

    internal async Task<TAggregateEntity?> FetchAsync(Guid aggregate, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TAggregate
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AggregateId == aggregate, token);
    }

    internal async Task<int> CountAsync(IAggregateCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .CountAsync(token);
    }

    internal async Task<IEnumerable<TAggregateEntity>> CollectAsync(IAggregateCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);
    }

    private IQueryable<TAggregateEntity> BuildQuery(IAggregateCriteria criteria)
    {
        using var db = _context.CreateDbContext();

        var query = db.TAggregate.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.AggregateId != null)
        //    query = query.Where(x => x.AggregateId == criteria.AggregateId);

        // if (criteria.AggregateType != null)
        //    query = query.Where(x => x.AggregateType == criteria.AggregateType);

        // if (criteria.AggregateRoot != null)
        //    query = query.Where(x => x.AggregateRoot == criteria.AggregateRoot);

        return query;
    }
}