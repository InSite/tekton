using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Security;

internal class TPermissionReader
{
    private readonly IDbContextFactory<TableDbContext> _context;

    internal TPermissionReader(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    internal async Task<bool> AssertAsync(Guid permission, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TPermission
            .AnyAsync(x => x.PermissionId == permission, token);
    }

    internal async Task<TPermissionEntity?> FetchAsync(Guid permission, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TPermission
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PermissionId == permission, token);
    }

    internal async Task<int> CountAsync(IPermissionCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .CountAsync(token);
    }

    internal async Task<IEnumerable<TPermissionEntity>> CollectAsync(IPermissionCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);
    }

    private IQueryable<TPermissionEntity> BuildQuery(IPermissionCriteria criteria)
    {
        using var db = _context.CreateDbContext();

        var query = db.TPermission.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.PermissionId != null)
        //    query = query.Where(x => x.PermissionId == criteria.PermissionId);

        // if (criteria.AccessType != null)
        //    query = query.Where(x => x.AccessType == criteria.AccessType);

        // if (criteria.AccessFlags != null)
        //    query = query.Where(x => x.AccessFlags == criteria.AccessFlags);

        // if (criteria.ResourceId != null)
        //    query = query.Where(x => x.ResourceId == criteria.ResourceId);

        // if (criteria.RoleId != null)
        //    query = query.Where(x => x.RoleId == criteria.RoleId);

        return query;
    }
}