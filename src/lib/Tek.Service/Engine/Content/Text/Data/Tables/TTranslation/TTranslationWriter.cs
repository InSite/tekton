using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Content;

public class TTranslationWriter
{
    private readonly IDbContextFactory<TableDbContext> _context;
    private readonly IValidator<TTranslationEntity> _validator;

    public TTranslationWriter(IDbContextFactory<TableDbContext> context,
        IValidator<TTranslationEntity> validator)
    {
        _context = context;
        _validator = validator;
    }

    public async Task<bool> CreateAsync(TTranslationEntity entity, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(entity, token);

        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.TranslationId, token, db);
        if (exists)
            return false;
                
        await db.TTranslation.AddAsync(entity, token);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> ModifyAsync(TTranslationEntity entity, CancellationToken token)
    {
        await _validator.ValidateAndThrowAsync(entity, token);
        
        using var db = _context.CreateDbContext();

        var exists = await AssertAsync(entity.TranslationId, token, db);
        if (!exists)
            return false;

        db.Entry(entity).State = EntityState.Modified;
        return await db.SaveChangesAsync(token) > 0;
    }
        
    public async Task<bool> DeleteAsync(Guid translation, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        var entity = await db.TTranslation.SingleOrDefaultAsync(x => x.TranslationId == translation, token);
        if (entity == null)
            return false;

        db.TTranslation.Remove(entity);
        return await db.SaveChangesAsync(token) > 0;
    }
        
    private async Task<bool> AssertAsync(Guid translation, CancellationToken token, TableDbContext db)
		=> await db.TTranslation.AsNoTracking().AnyAsync(x => x.TranslationId == translation, token);
}