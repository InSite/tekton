using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Security;

public class TPermissionReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly IValidator<IPermissionCriteria> _validator;
    private readonly PermissionAdapter _adapter;

    public TPermissionReader(IDbContextFactory<TableDbContext> context, 
        IValidator<IPermissionCriteria> validator, PermissionAdapter adapter)
    {
        _context = context;
        _validator = validator;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(Guid permission, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TPermission
            .AnyAsync(x => x.PermissionId == permission, token);
    }

    public async Task<TPermissionEntity?> FetchAsync(Guid permission, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TPermission
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PermissionId == permission, token);
    }

    public async Task<int> CountAsync(IPermissionCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TPermissionEntity>> CollectAsync(IPermissionCriteria criteria, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(criteria, token);
        
        return await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<PermissionMatch>> SearchAsync(IPermissionCriteria criteria, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(criteria, token);
        
        var entities = await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
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