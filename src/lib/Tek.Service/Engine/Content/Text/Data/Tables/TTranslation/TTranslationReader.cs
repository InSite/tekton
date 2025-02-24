using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Content;

internal class TTranslationReader
{
    private readonly IDbContextFactory<TableDbContext> _context;

    internal TTranslationReader(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    internal async Task<bool> AssertAsync(Guid translation, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TTranslation
            .AnyAsync(x => x.TranslationId == translation, token);
    }

    internal async Task<TTranslationEntity?> FetchAsync(Guid translation, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TTranslation
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TranslationId == translation, token);
    }

    internal async Task<int> CountAsync(ITranslationCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .CountAsync(token);
    }

    internal async Task<IEnumerable<TTranslationEntity>> CollectAsync(ITranslationCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);
    }

    private IQueryable<TTranslationEntity> BuildQuery(ITranslationCriteria criteria)
    {
        using var db = _context.CreateDbContext();

        var query = db.TTranslation.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.TranslationId != null)
        //    query = query.Where(x => x.TranslationId == criteria.TranslationId);

        // if (criteria.TranslationText != null)
        //    query = query.Where(x => x.TranslationText == criteria.TranslationText);

        // if (criteria.ModifiedWhen != null)
        //    query = query.Where(x => x.ModifiedWhen == criteria.ModifiedWhen);

        return query;
    }
}