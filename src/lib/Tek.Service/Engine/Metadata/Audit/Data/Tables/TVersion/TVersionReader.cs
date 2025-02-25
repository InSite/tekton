using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Metadata;

public class TVersionReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly IValidator<IVersionCriteria> _validator;
    private readonly VersionAdapter _adapter;

    public TVersionReader(IDbContextFactory<TableDbContext> context, 
        IValidator<IVersionCriteria> validator, VersionAdapter adapter)
    {
        _context = context;
        _validator = validator;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(int versionNumber, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TVersion
            .AnyAsync(x => x.VersionNumber == versionNumber, token);
    }

    public async Task<TVersionEntity?> FetchAsync(int versionNumber, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TVersion
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.VersionNumber == versionNumber, token);
    }

    public async Task<int> CountAsync(IVersionCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TVersionEntity>> CollectAsync(IVersionCriteria criteria, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(criteria, token);
        
        return await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<VersionMatch>> SearchAsync(IVersionCriteria criteria, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(criteria, token);
        
        var entities = await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
    }

    private IQueryable<TVersionEntity> BuildQuery(IVersionCriteria criteria)
    {
        using var db = _context.CreateDbContext();

        var query = db.TVersion.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.VersionNumber != null)
        //    query = query.Where(x => x.VersionNumber == criteria.VersionNumber);

        // if (criteria.VersionType != null)
        //    query = query.Where(x => x.VersionType == criteria.VersionType);

        // if (criteria.VersionName != null)
        //    query = query.Where(x => x.VersionName == criteria.VersionName);

        // if (criteria.ScriptPath != null)
        //    query = query.Where(x => x.ScriptPath == criteria.ScriptPath);

        // if (criteria.ScriptContent != null)
        //    query = query.Where(x => x.ScriptContent == criteria.ScriptContent);

        // if (criteria.ScriptExecuted != null)
        //    query = query.Where(x => x.ScriptExecuted == criteria.ScriptExecuted);

        return query;
    }
}