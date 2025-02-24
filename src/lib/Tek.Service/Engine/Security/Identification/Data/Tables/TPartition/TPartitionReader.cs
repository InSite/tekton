using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Security;

internal class TPartitionReader
{
    private readonly IDbContextFactory<TableDbContext> _context;

    internal TPartitionReader(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    internal async Task<bool> AssertAsync(int partitionNumber, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TPartition
            .AnyAsync(x => x.PartitionNumber == partitionNumber, token);
    }

    internal async Task<TPartitionEntity?> FetchAsync(int partitionNumber, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TPartition
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PartitionNumber == partitionNumber, token);
    }

    internal async Task<int> CountAsync(IPartitionCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .CountAsync(token);
    }

    internal async Task<IEnumerable<TPartitionEntity>> CollectAsync(IPartitionCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);
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