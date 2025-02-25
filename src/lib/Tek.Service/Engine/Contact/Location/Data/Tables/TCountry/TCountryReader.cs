using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Contact;

public class TCountryReader
{
    private readonly IDbContextFactory<TableDbContext> _context;

    public TCountryReader(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    public async Task<bool> AssertAsync(Guid country, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TCountry
            .AnyAsync(x => x.CountryId == country, token);
    }

    public async Task<TCountryEntity?> FetchAsync(Guid country, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TCountry
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CountryId == country, token);
    }

    public async Task<int> CountAsync(ICountryCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TCountryEntity>> CollectAsync(ICountryCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);
    }

    private IQueryable<TCountryEntity> BuildQuery(ICountryCriteria criteria)
    {
        using var db = _context.CreateDbContext();

        var query = db.TCountry.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.CountryId != null)
        //    query = query.Where(x => x.CountryId == criteria.CountryId);

        // if (criteria.CountryCode != null)
        //    query = query.Where(x => x.CountryCode == criteria.CountryCode);

        // if (criteria.CountryName != null)
        //    query = query.Where(x => x.CountryName == criteria.CountryName);

        // if (criteria.Languages != null)
        //    query = query.Where(x => x.Languages == criteria.Languages);

        // if (criteria.CurrencyCode != null)
        //    query = query.Where(x => x.CurrencyCode == criteria.CurrencyCode);

        // if (criteria.CurrencyName != null)
        //    query = query.Where(x => x.CurrencyName == criteria.CurrencyName);

        // if (criteria.TopLevelDomain != null)
        //    query = query.Where(x => x.TopLevelDomain == criteria.TopLevelDomain);

        // if (criteria.ContinentCode != null)
        //    query = query.Where(x => x.ContinentCode == criteria.ContinentCode);

        // if (criteria.CapitalCityName != null)
        //    query = query.Where(x => x.CapitalCityName == criteria.CapitalCityName);

        return query;
    }
}