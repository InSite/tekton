using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Security;

public class TPartitionReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly IValidator<IPartitionCriteria> _validator;
    private readonly PartitionAdapter _adapter;

    public TPartitionReader(IDbContextFactory<TableDbContext> context, 
        IValidator<IPartitionCriteria> validator, PartitionAdapter adapter)
    {
        _context = context;
        _validator = validator;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(int partitionNumber, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TPartition
            .AnyAsync(x => x.PartitionNumber == partitionNumber, token);
    }

    public async Task<TPartitionEntity?> FetchAsync(int partitionNumber, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TPartition
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PartitionNumber == partitionNumber, token);
    }

    public async Task<int> CountAsync(IPartitionCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TPartitionEntity>> CollectAsync(IPartitionCriteria criteria, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(criteria, token);
        
        return await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<PartitionMatch>> SearchAsync(IPartitionCriteria criteria, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(criteria, token);
        
        var entities = await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
    }

    private IQueryable<TPartitionEntity> BuildQuery(IPartitionCriteria criteria)
    {
        using var db = _context.CreateDbContext();

        var query = db.TPartition.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.PartitionNumber != null)
        //    query = query.Where(x => x.PartitionNumber == criteria.PartitionNumber);

        // if (criteria.PartitionSlug != null)
        //    query = query.Where(x => x.PartitionSlug == criteria.PartitionSlug);

        // if (criteria.PartitionName != null)
        //    query = query.Where(x => x.PartitionName == criteria.PartitionName);

        // if (criteria.PartitionHost != null)
        //    query = query.Where(x => x.PartitionHost == criteria.PartitionHost);

        // if (criteria.PartitionEmail != null)
        //    query = query.Where(x => x.PartitionEmail == criteria.PartitionEmail);

        // if (criteria.PartitionSettings != null)
        //    query = query.Where(x => x.PartitionSettings == criteria.PartitionSettings);

        // if (criteria.PartitionTesters != null)
        //    query = query.Where(x => x.PartitionTesters == criteria.PartitionTesters);

        // if (criteria.ModifiedWhen != null)
        //    query = query.Where(x => x.ModifiedWhen == criteria.ModifiedWhen);

        return query;
    }
}