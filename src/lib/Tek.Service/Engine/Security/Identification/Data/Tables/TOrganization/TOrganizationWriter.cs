using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Security;

public class TOrganizationWriter
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly IValidator<TOrganizationEntity> _validator;

    public TOrganizationWriter(IDbContextFactory<TableDbContext> context,
        IValidator<TOrganizationEntity> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<bool> CreateAsync(TOrganizationEntity entity, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(entity, token);

        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.OrganizationId, token, db);
        if (exists)
            return false;
                
        await db.TOrganization.AddAsync(entity, token);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> ModifyAsync(TOrganizationEntity entity, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(entity, token);
        
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.OrganizationId, token, db);
        if (!exists)
            return false;

        db.Entry(entity).State = EntityState.Modified;
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> DeleteAsync(Guid organization, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entity = await db.TOrganization.SingleOrDefaultAsync(x => x.OrganizationId == organization, token);
        if (entity == null)
            return false;

        db.TOrganization.Remove(entity);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    private async Task<bool> AssertAsync(Guid organization, CancellationToken token, TableDbContext db)
		=> await db.TOrganization.AsNoTracking().AnyAsync(x => x.OrganizationId == organization, token);
}