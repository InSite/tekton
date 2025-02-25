using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Bus;

public class TAggregateReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly IValidator<IAggregateCriteria> _validator;
    private readonly AggregateAdapter _adapter;

    public TAggregateReader(IDbContextFactory<TableDbContext> context, 
        IValidator<IAggregateCriteria> validator, AggregateAdapter adapter)
    {
        _context = context;
        _validator = validator;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(Guid aggregate, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TAggregate
            .AnyAsync(x => x.AggregateId == aggregate, token);
    }

    public async Task<TAggregateEntity?> FetchAsync(Guid aggregate, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TAggregate
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AggregateId == aggregate, token);
    }

    public async Task<int> CountAsync(IAggregateCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TAggregateEntity>> CollectAsync(IAggregateCriteria criteria, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(criteria, token);
        
        return await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<AggregateMatch>> SearchAsync(IAggregateCriteria criteria, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(criteria, token);
        
        var entities = await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
    }

    private IQueryable<TAggregateEntity> BuildQuery(IAggregateCriteria criteria)
    {
        using var db = _context.CreateDbContext();

        var query = db.TAggregate.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.AggregateId != null)
        //    query = query.Where(x => x.AggregateId == criteria.AggregateId);

        // if (criteria.AggregateType != null)
        //    query = query.Where(x => x.AggregateType == criteria.AggregateType);

        // if (criteria.AggregateRoot != null)
        //    query = query.Where(x => x.AggregateRoot == criteria.AggregateRoot);

        return query;
    }
}