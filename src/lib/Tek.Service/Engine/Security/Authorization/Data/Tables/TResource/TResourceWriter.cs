using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Security;

public class TResourceWriter
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly IValidator<TResourceEntity> _validator;

    public TResourceWriter(IDbContextFactory<TableDbContext> context,
        IValidator<TResourceEntity> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<bool> CreateAsync(TResourceEntity entity, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(entity, token);

        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.ResourceId, token, db);
        if (exists)
            return false;
                
        await db.TResource.AddAsync(entity, token);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> ModifyAsync(TResourceEntity entity, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(entity, token);
        
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.ResourceId, token, db);
        if (!exists)
            return false;

        db.Entry(entity).State = EntityState.Modified;
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> DeleteAsync(Guid resource, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entity = await db.TResource.SingleOrDefaultAsync(x => x.ResourceId == resource, token);
        if (entity == null)
            return false;

        db.TResource.Remove(entity);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    private async Task<bool> AssertAsync(Guid resource, CancellationToken token, TableDbContext db)
		=> await db.TResource.AsNoTracking().AnyAsync(x => x.ResourceId == resource, token);
}