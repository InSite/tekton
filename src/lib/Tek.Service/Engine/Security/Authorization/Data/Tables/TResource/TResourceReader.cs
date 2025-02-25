using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Security;

public class TResourceReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly IValidator<IResourceCriteria> _validator;
    private readonly ResourceAdapter _adapter;

    public TResourceReader(IDbContextFactory<TableDbContext> context, 
        IValidator<IResourceCriteria> validator, ResourceAdapter adapter)
    {
        _context = context;
        _validator = validator;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(Guid resource, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TResource
            .AnyAsync(x => x.ResourceId == resource, token);
    }

    public async Task<TResourceEntity?> FetchAsync(Guid resource, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TResource
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ResourceId == resource, token);
    }

    public async Task<int> CountAsync(IResourceCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TResourceEntity>> CollectAsync(IResourceCriteria criteria, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(criteria, token);
        
        return await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<ResourceMatch>> SearchAsync(IResourceCriteria criteria, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(criteria, token);
        
        var entities = await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
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