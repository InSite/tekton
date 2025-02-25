using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Security;

public class ResourceService
{
    private readonly TResourceReader _reader;
    private readonly TResourceWriter _writer;

    private readonly ResourceAdapter _adapter = new ResourceAdapter();

    private readonly IValidator<IResourceCriteria> _criteriaValidator;
    private readonly IValidator<TResourceEntity> _entityValidator;

    public ResourceService(TResourceReader reader, TResourceWriter writer,
        IValidator<IResourceCriteria> criteriaValidator, IValidator<TResourceEntity> entityValidator)
    {
        _reader = reader;
        _writer = writer;

        _criteriaValidator = criteriaValidator;
        _entityValidator = entityValidator;
    }

    public async Task<bool> AssertAsync(Guid resource, CancellationToken token)
        => await _reader.AssertAsync(resource, token);

    public async Task<int> CountAsync(IResourceCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<ResourceModel?> FetchAsync(Guid resource, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(resource, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<IEnumerable<ResourceModel>> CollectAsync(IResourceCriteria criteria, CancellationToken token)
    {
        await _criteriaValidator.ValidateAndThrowAsync(criteria, token);

        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<ResourceMatch>> SearchAsync(IResourceCriteria criteria, CancellationToken token)
    {
        await _criteriaValidator.ValidateAndThrowAsync(criteria, token);

        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToMatch(entities);
    }

    public async Task<bool> CreateAsync(CreateResource create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        await _entityValidator.ValidateAndThrowAsync(entity, token);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyResource modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.ResourceId, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        await _entityValidator.ValidateAndThrowAsync(entity, token);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(Guid resource, CancellationToken token)
        => await _writer.DeleteAsync(resource, token);
}