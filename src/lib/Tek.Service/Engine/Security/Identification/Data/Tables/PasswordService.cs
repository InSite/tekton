using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Security;

public class PasswordService
{
    private readonly TPasswordReader _reader;
    private readonly TPasswordWriter _writer;

    private readonly PasswordAdapter _adapter = new PasswordAdapter();

    private readonly IValidator<IPasswordCriteria> _criteriaValidator;
    private readonly IValidator<TPasswordEntity> _entityValidator;

    public PasswordService(TPasswordReader reader, TPasswordWriter writer,
        IValidator<IPasswordCriteria> criteriaValidator, IValidator<TPasswordEntity> entityValidator)
    {
        _reader = reader;
        _writer = writer;

        _criteriaValidator = criteriaValidator;
        _entityValidator = entityValidator;
    }

    public async Task<bool> AssertAsync(Guid password, CancellationToken token)
        => await _reader.AssertAsync(password, token);

    public async Task<int> CountAsync(IPasswordCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<PasswordModel?> FetchAsync(Guid password, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(password, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<IEnumerable<PasswordModel>> CollectAsync(IPasswordCriteria criteria, CancellationToken token)
    {
        await _criteriaValidator.ValidateAndThrowAsync(criteria, token);

        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<PasswordMatch>> SearchAsync(IPasswordCriteria criteria, CancellationToken token)
    {
        await _criteriaValidator.ValidateAndThrowAsync(criteria, token);

        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToMatch(entities);
    }

    public async Task<bool> CreateAsync(CreatePassword create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        await _entityValidator.ValidateAndThrowAsync(entity, token);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyPassword modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.PasswordId, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        await _entityValidator.ValidateAndThrowAsync(entity, token);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(Guid password, CancellationToken token)
        => await _writer.DeleteAsync(password, token);
}