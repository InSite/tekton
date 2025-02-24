using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Security;

internal class TResourceReader
{
    private readonly IDbContextFactory<TableDbContext> _context;

    internal TResourceReader(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    internal async Task<bool> AssertAsync(Guid resource, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TResource
            .AnyAsync(x => x.ResourceId == resource, token);
    }

    internal async Task<TResourceEntity?> FetchAsync(Guid resource, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TResource
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ResourceId == resource, token);
    }

    internal async Task<int> CountAsync(IResourceCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .CountAsync(token);
    }

    internal async Task<IEnumerable<TResourceEntity>> CollectAsync(IResourceCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);
    }

    private IQueryable<TResourceEntity> BuildQuery(IResourceCriteria criteria)
    {
        using var db = _context.CreateDbContext();

        var query = db.TResource.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.ResourceId != null)
        //    query = query.Where(x => x.ResourceId == criteria.ResourceId);

        // if (criteria.ResourceType != null)
        //    query = query.Where(x => x.ResourceType == criteria.ResourceType);

        // if (criteria.ResourceName != null)
        //    query = query.Where(x => x.ResourceName == criteria.ResourceName);

        return query;
    }
}