using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Security;

public class TRoleReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly IValidator<IRoleCriteria> _validator;
    private readonly RoleAdapter _adapter;

    public TRoleReader(IDbContextFactory<TableDbContext> context, 
        IValidator<IRoleCriteria> validator, RoleAdapter adapter)
    {
        _context = context;
        _validator = validator;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(Guid role, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TRole
            .AnyAsync(x => x.RoleId == role, token);
    }

    public async Task<TRoleEntity?> FetchAsync(Guid role, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TRole
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.RoleId == role, token);
    }

    public async Task<int> CountAsync(IRoleCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TRoleEntity>> CollectAsync(IRoleCriteria criteria, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(criteria, token);
        
        return await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<RoleMatch>> SearchAsync(IRoleCriteria criteria, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(criteria, token);
        
        var entities = await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
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