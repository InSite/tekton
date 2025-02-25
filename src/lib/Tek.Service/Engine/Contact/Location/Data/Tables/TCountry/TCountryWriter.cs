using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Contact;

public class TCountryWriter
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly IValidator<TCountryEntity> _validator;

    public TCountryWriter(IDbContextFactory<TableDbContext> context,
        IValidator<TCountryEntity> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<bool> CreateAsync(TCountryEntity entity, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(entity, token);

        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.CountryId, token, db);
        if (exists)
            return false;
                
        await db.TCountry.AddAsync(entity, token);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> ModifyAsync(TCountryEntity entity, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(entity, token);
        
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.CountryId, token, db);
        if (!exists)
            return false;

        db.Entry(entity).State = EntityState.Modified;
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> DeleteAsync(Guid country, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entity = await db.TCountry.SingleOrDefaultAsync(x => x.CountryId == country, token);
        if (entity == null)
            return false;

        db.TCountry.Remove(entity);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    private async Task<bool> AssertAsync(Guid country, CancellationToken token, TableDbContext db)
		=> await db.TCountry.AsNoTracking().AnyAsync(x => x.CountryId == country, token);
}