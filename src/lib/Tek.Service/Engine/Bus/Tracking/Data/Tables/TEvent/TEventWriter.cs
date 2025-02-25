using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Bus;

public class TEventWriter
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly IValidator<TEventEntity> _validator;

    public TEventWriter(IDbContextFactory<TableDbContext> context,
        IValidator<TEventEntity> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<bool> CreateAsync(TEventEntity entity, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(entity, token);

        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.EventId, token, db);
        if (exists)
            return false;
                
        await db.TEvent.AddAsync(entity, token);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> ModifyAsync(TEventEntity entity, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(entity, token);
        
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.EventId, token, db);
        if (!exists)
            return false;

        db.Entry(entity).State = EntityState.Modified;
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> DeleteAsync(Guid @event, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entity = await db.TEvent.SingleOrDefaultAsync(x => x.EventId == @event, token);
        if (entity == null)
            return false;

        db.TEvent.Remove(entity);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    private async Task<bool> AssertAsync(Guid @event, CancellationToken token, TableDbContext db)
		=> await db.TEvent.AsNoTracking().AnyAsync(x => x.EventId == @event, token);
}