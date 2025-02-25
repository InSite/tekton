using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Security;

public class PermissionService
{
    private readonly TPermissionReader _reader;
    private readonly TPermissionWriter _writer;

    private readonly PermissionAdapter _adapter = new PermissionAdapter();

    private readonly IValidator<IPermissionCriteria> _criteriaValidator;
    private readonly IValidator<TPermissionEntity> _entityValidator;

    public PermissionService(TPermissionReader reader, TPermissionWriter writer,
        IValidator<IPermissionCriteria> criteriaValidator, IValidator<TPermissionEntity> entityValidator)
    {
        _reader = reader;
        _writer = writer;

        _criteriaValidator = criteriaValidator;
        _entityValidator = entityValidator;
    }

    public async Task<bool> AssertAsync(Guid permission, CancellationToken token)
        => await _reader.AssertAsync(permission, token);

    public async Task<int> CountAsync(IPermissionCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<PermissionModel?> FetchAsync(Guid permission, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(permission, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<IEnumerable<PermissionModel>> CollectAsync(IPermissionCriteria criteria, CancellationToken token)
    {
        await _criteriaValidator.ValidateAndThrowAsync(criteria, token);

        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<PermissionMatch>> SearchAsync(IPermissionCriteria criteria, CancellationToken token)
    {
        await _criteriaValidator.ValidateAndThrowAsync(criteria, token);

        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToMatch(entities);
    }

    public async Task<bool> CreateAsync(CreatePermission create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        await _entityValidator.ValidateAndThrowAsync(entity, token);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyPermission modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.PermissionId, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        await _entityValidator.ValidateAndThrowAsync(entity, token);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(Guid permission, CancellationToken token)
        => await _writer.DeleteAsync(permission, token);
}