using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Security;

public class TPasswordReader
{
    private readonly IDbContextFactory<TableDbContext> _context;

    public TPasswordReader(IDbContextFactory<TableDbContext> context)
    {
        _context = context;
    }

    public async Task<bool> AssertAsync(Guid password, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TPassword
            .AnyAsync(x => x.PasswordId == password, token);
    }

    public async Task<TPasswordEntity?> FetchAsync(Guid password, CancellationToken token)
    {
        using var db = _context.CreateDbContext();

        return await db.TPassword
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.PasswordId == password, token);
    }

    public async Task<int> CountAsync(IPasswordCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .CountAsync(token);
    }

    public async Task<IEnumerable<TPasswordEntity>> CollectAsync(IPasswordCriteria criteria, CancellationToken token)
    {
        return await BuildQuery(criteria)
            .Skip((criteria.Filter.Page - 1) * criteria.Filter.Take)
            .Take(criteria.Filter.Take)
            .ToListAsync(token);
    }

    private IQueryable<TPasswordEntity> BuildQuery(IPasswordCriteria criteria)
    {
        using var db = _context.CreateDbContext();

        var query = db.TPassword.AsNoTracking().AsQueryable();

        // TODO: Implement search criteria

        // if (criteria.PasswordId != null)
        //    query = query.Where(x => x.PasswordId == criteria.PasswordId);

        // if (criteria.EmailId != null)
        //    query = query.Where(x => x.EmailId == criteria.EmailId);

        // if (criteria.EmailAddress != null)
        //    query = query.Where(x => x.EmailAddress == criteria.EmailAddress);

        // if (criteria.PasswordHash != null)
        //    query = query.Where(x => x.PasswordHash == criteria.PasswordHash);

        // if (criteria.PasswordExpiry != null)
        //    query = query.Where(x => x.PasswordExpiry == criteria.PasswordExpiry);

        // if (criteria.DefaultPlaintext != null)
        //    query = query.Where(x => x.DefaultPlaintext == criteria.DefaultPlaintext);

        // if (criteria.DefaultExpiry != null)
        //    query = query.Where(x => x.DefaultExpiry == criteria.DefaultExpiry);

        // if (criteria.CreatedWhen != null)
        //    query = query.Where(x => x.CreatedWhen == criteria.CreatedWhen);

        // if (criteria.LastForgottenWhen != null)
        //    query = query.Where(x => x.LastForgottenWhen == criteria.LastForgottenWhen);

        // if (criteria.LastModifiedWhen != null)
        //    query = query.Where(x => x.LastModifiedWhen == criteria.LastModifiedWhen);

        return query;
    }
}