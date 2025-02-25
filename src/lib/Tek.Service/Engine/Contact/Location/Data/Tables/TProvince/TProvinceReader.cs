using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Contact;

public class TProvinceReader
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly IValidator<IProvinceCriteria> _validator;
    private readonly ProvinceAdapter _adapter;

    public TProvinceReader(IDbContextFactory<TableDbContext> context, 
        IValidator<IProvinceCriteria> validator, ProvinceAdapter adapter)
    {
        _context = context;
        _validator = validator;
        _adapter = adapter;
    }

    public async Task<bool> AssertAsync(Guid province, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TProvince
            .AnyAsync(x => x.ProvinceId == province, token);
    }

    public async Task<TProvinceEntity?> FetchAsync(Guid province, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TProvince
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ProvinceId == province, token);
    }

    public async Task<int> CountAsync(IProvinceCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TProvinceEntity>> CollectAsync(IProvinceCriteria criteria, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(criteria, token);
        
        return await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);
    }

    public async Task<IEnumerable<ProvinceMatch>> SearchAsync(IProvinceCriteria criteria, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(criteria, token);
        
        var entities = await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);

        return _adapter.ToMatch(entities);
    }

    private IQueryable<TProvinceEntity> BuildQuery(IProvinceCriteria criteria)
    {
        using var db = _context.CreateDbContext();

        var query = db.TProvince.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.ProvinceId != null)
        //    query = query.Where(x => x.ProvinceId == criteria.ProvinceId);

        // if (criteria.ProvinceCode != null)
        //    query = query.Where(x => x.ProvinceCode == criteria.ProvinceCode);

        // if (criteria.ProvinceName != null)
        //    query = query.Where(x => x.ProvinceName == criteria.ProvinceName);

        // if (criteria.CountryCode != null)
        //    query = query.Where(x => x.CountryCode == criteria.CountryCode);

        // if (criteria.CountryId != null)
        //    query = query.Where(x => x.CountryId == criteria.CountryId);

        return query;
    }
}