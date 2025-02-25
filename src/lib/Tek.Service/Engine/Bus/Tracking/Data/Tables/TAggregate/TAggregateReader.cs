using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Bus;

public class TAggregateReader
{
    private readonly IDbContextFactory<TableDbContext> _context;

    public TAggregateReader(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    public async Task<bool> AssertAsync(Guid aggregate, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TAggregate
            .AnyAsync(x => x.AggregateId == aggregate, token);
    }

    public async Task<TAggregateEntity?> FetchAsync(Guid aggregate, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TAggregate
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AggregateId == aggregate, token);
    }

    public async Task<int> CountAsync(IAggregateCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TAggregateEntity>> CollectAsync(IAggregateCriteria criteria, CancellationToken token)
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