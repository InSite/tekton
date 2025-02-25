using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Content;

public class TTranslationReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly IValidator<ITranslationCriteria> _validator;
    private readonly TranslationAdapter _adapter;

    public TTranslationReader(IDbContextFactory<TableDbContext> context, 
        IValidator<ITranslationCriteria> validator, TranslationAdapter adapter)
    {
        _context = context;
        _validator = validator;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(Guid translation, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TTranslation
            .AnyAsync(x => x.TranslationId == translation, token);
    }

    public async Task<TTranslationEntity?> FetchAsync(Guid translation, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TTranslation
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TranslationId == translation, token);
    }

    public async Task<int> CountAsync(ITranslationCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TTranslationEntity>> CollectAsync(ITranslationCriteria criteria, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(criteria, token);
        
        return await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<TranslationMatch>> SearchAsync(ITranslationCriteria criteria, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(criteria, token);
        
        var entities = await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
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