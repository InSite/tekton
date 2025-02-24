using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Security;

internal class TRoleReader
{
    private readonly IDbContextFactory<TableDbContext> _context;

    internal TRoleReader(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    internal async Task<bool> AssertAsync(Guid role, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TRole
            .AnyAsync(x => x.RoleId == role, token);
    }

    internal async Task<TRoleEntity?> FetchAsync(Guid role, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TRole
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.RoleId == role, token);
    }

    internal async Task<int> CountAsync(IRoleCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .CountAsync(token);
    }

    internal async Task<IEnumerable<TRoleEntity>> CollectAsync(IRoleCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);
    }

    private IQueryable<TRoleEntity> BuildQuery(IRoleCriteria criteria)
    {
        using var db = _context.CreateDbContext();

        var query = db.TRole.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.RoleId != null)
        //    query = query.Where(x => x.RoleId == criteria.RoleId);

        // if (criteria.RoleType != null)
        //    query = query.Where(x => x.RoleType == criteria.RoleType);

        // if (criteria.RoleName != null)
        //    query = query.Where(x => x.RoleName == criteria.RoleName);

        return query;
    }
}