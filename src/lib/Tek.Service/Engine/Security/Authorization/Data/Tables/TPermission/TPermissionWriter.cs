using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Security;

public class TPermissionWriter
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly IValidator<TPermissionEntity> _validator;

    public TPermissionWriter(IDbContextFactory<TableDbContext> context,
        IValidator<TPermissionEntity> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<bool> CreateAsync(TPermissionEntity entity, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(entity, token);

        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.PermissionId, token, db);
        if (exists)
            return false;
                
        await db.TPermission.AddAsync(entity, token);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> ModifyAsync(TPermissionEntity entity, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(entity, token);
        
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.PermissionId, token, db);
        if (!exists)
            return false;

        db.Entry(entity).State = EntityState.Modified;
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> DeleteAsync(Guid permission, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entity = await db.TPermission.SingleOrDefaultAsync(x => x.PermissionId == permission, token);
        if (entity == null)
            return false;

        db.TPermission.Remove(entity);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    private async Task<bool> AssertAsync(Guid permission, CancellationToken token, TableDbContext db)
		=> await db.TPermission.AsNoTracking().AnyAsync(x => x.PermissionId == permission, token);
}