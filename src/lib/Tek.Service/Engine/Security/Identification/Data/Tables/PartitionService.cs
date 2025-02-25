using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Security;

public class PartitionService
{
    private readonly TPartitionReader _reader;
    private readonly TPartitionWriter _writer;

    private readonly PartitionAdapter _adapter = new PartitionAdapter();

    private readonly IValidator<IPartitionCriteria> _criteriaValidator;
    private readonly IValidator<TPartitionEntity> _entityValidator;

    public PartitionService(TPartitionReader reader, TPartitionWriter writer,
        IValidator<IPartitionCriteria> criteriaValidator, IValidator<TPartitionEntity> entityValidator)
    {
        _reader = reader;
        _writer = writer;

        _criteriaValidator = criteriaValidator;
        _entityValidator = entityValidator;
    }

    public async Task<bool> AssertAsync(int partitionNumber, CancellationToken token)
        => await _reader.AssertAsync(partitionNumber, token);

    public async Task<int> CountAsync(IPartitionCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<PartitionModel?> FetchAsync(int partitionNumber, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(partitionNumber, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<IEnumerable<PartitionModel>> CollectAsync(IPartitionCriteria criteria, CancellationToken token)
    {
        await _criteriaValidator.ValidateAndThrowAsync(criteria, token);

        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<PartitionMatch>> SearchAsync(IPartitionCriteria criteria, CancellationToken token)
    {
        await _criteriaValidator.ValidateAndThrowAsync(criteria, token);

        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToMatch(entities);
    }

    public async Task<bool> CreateAsync(CreatePartition create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        await _entityValidator.ValidateAndThrowAsync(entity, token);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyPartition modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.PartitionNumber, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        await _entityValidator.ValidateAndThrowAsync(entity, token);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(int partitionNumber, CancellationToken token)
        => await _writer.DeleteAsync(partitionNumber, token);
}