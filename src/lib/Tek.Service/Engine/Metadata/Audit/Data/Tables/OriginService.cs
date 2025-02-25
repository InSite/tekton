using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Metadata;

public class OriginService
{
    private readonly TOriginReader _reader;
    private readonly TOriginWriter _writer;

    private readonly OriginAdapter _adapter = new OriginAdapter();

    private readonly IValidator<IOriginCriteria> _criteriaValidator;
    private readonly IValidator<TOriginEntity> _entityValidator;

    public OriginService(TOriginReader reader, TOriginWriter writer,
        IValidator<IOriginCriteria> criteriaValidator, IValidator<TOriginEntity> entityValidator)
    {
        _reader = reader;
        _writer = writer;

        _criteriaValidator = criteriaValidator;
        _entityValidator = entityValidator;
    }

    public async Task<bool> AssertAsync(Guid origin, CancellationToken token)
        => await _reader.AssertAsync(origin, token);

    public async Task<int> CountAsync(IOriginCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<OriginModel?> FetchAsync(Guid origin, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(origin, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<IEnumerable<OriginModel>> CollectAsync(IOriginCriteria criteria, CancellationToken token)
    {
        await _criteriaValidator.ValidateAndThrowAsync(criteria, token);

        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<OriginMatch>> SearchAsync(IOriginCriteria criteria, CancellationToken token)
    {
        await _criteriaValidator.ValidateAndThrowAsync(criteria, token);

        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToMatch(entities);
    }

    public async Task<bool> CreateAsync(CreateOrigin create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        await _entityValidator.ValidateAndThrowAsync(entity, token);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyOrigin modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.OriginId, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        await _entityValidator.ValidateAndThrowAsync(entity, token);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(Guid origin, CancellationToken token)
        => await _writer.DeleteAsync(origin, token);
}