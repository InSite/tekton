using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Metadata;

public class TOriginReader
{
    private readonly IDbContextFactory<TableDbContext> _context;

    public TOriginReader(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    public async Task<bool> AssertAsync(Guid origin, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TOrigin
            .AnyAsync(x => x.OriginId == origin, token);
    }

    public async Task<TOriginEntity?> FetchAsync(Guid origin, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TOrigin
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.OriginId == origin, token);
    }

    public async Task<int> CountAsync(IOriginCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TOriginEntity>> CollectAsync(IOriginCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);
    }

    private IQueryable<TOriginEntity> BuildQuery(IOriginCriteria criteria)
    {
        using var db = _context.CreateDbContext();

        var query = db.TOrigin.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.OriginId != null)
        //    query = query.Where(x => x.OriginId == criteria.OriginId);

        // if (criteria.OriginWhen != null)
        //    query = query.Where(x => x.OriginWhen == criteria.OriginWhen);

        // if (criteria.OriginDescription != null)
        //    query = query.Where(x => x.OriginDescription == criteria.OriginDescription);

        // if (criteria.OriginReason != null)
        //    query = query.Where(x => x.OriginReason == criteria.OriginReason);

        // if (criteria.OriginSource != null)
        //    query = query.Where(x => x.OriginSource == criteria.OriginSource);

        // if (criteria.UserId != null)
        //    query = query.Where(x => x.UserId == criteria.UserId);

        // if (criteria.OrganizationId != null)
        //    query = query.Where(x => x.OrganizationId == criteria.OrganizationId);

        // if (criteria.ProxyAgent != null)
        //    query = query.Where(x => x.ProxyAgent == criteria.ProxyAgent);

        // if (criteria.ProxySubject != null)
        //    query = query.Where(x => x.ProxySubject == criteria.ProxySubject);

        return query;
    }
}