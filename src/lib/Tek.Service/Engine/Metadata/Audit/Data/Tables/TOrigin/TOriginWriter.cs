using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Metadata;

public class TOriginWriter
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly IValidator<TOriginEntity> _validator;

    public TOriginWriter(IDbContextFactory<TableDbContext> context,
        IValidator<TOriginEntity> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<bool> CreateAsync(TOriginEntity entity, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(entity, token);

        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.OriginId, token, db);
        if (exists)
            return false;
                
        await db.TOrigin.AddAsync(entity, token);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> ModifyAsync(TOriginEntity entity, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(entity, token);
        
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.OriginId, token, db);
        if (!exists)
            return false;

        db.Entry(entity).State = EntityState.Modified;
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> DeleteAsync(Guid origin, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entity = await db.TOrigin.SingleOrDefaultAsync(x => x.OriginId == origin, token);
        if (entity == null)
            return false;

        db.TOrigin.Remove(entity);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    private async Task<bool> AssertAsync(Guid origin, CancellationToken token, TableDbContext db)
		=> await db.TOrigin.AsNoTracking().AnyAsync(x => x.OriginId == origin, token);
}