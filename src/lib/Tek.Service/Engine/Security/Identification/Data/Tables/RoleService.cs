using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Security;

public class RoleService
{
    private readonly TRoleReader _reader;
    private readonly TRoleWriter _writer;

    private readonly RoleAdapter _adapter = new RoleAdapter();

    private readonly IValidator<IRoleCriteria> _criteriaValidator;
    private readonly IValidator<TRoleEntity> _entityValidator;

    public RoleService(TRoleReader reader, TRoleWriter writer,
        IValidator<IRoleCriteria> criteriaValidator, IValidator<TRoleEntity> entityValidator)
    {
        _reader = reader;
        _writer = writer;

        _criteriaValidator = criteriaValidator;
        _entityValidator = entityValidator;
    }

    public async Task<bool> AssertAsync(Guid role, CancellationToken token)
        => await _reader.AssertAsync(role, token);

    public async Task<int> CountAsync(IRoleCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<RoleModel?> FetchAsync(Guid role, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(role, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<IEnumerable<RoleModel>> CollectAsync(IRoleCriteria criteria, CancellationToken token)
    {
        await _criteriaValidator.ValidateAndThrowAsync(criteria, token);

        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<RoleMatch>> SearchAsync(IRoleCriteria criteria, CancellationToken token)
    {
        await _criteriaValidator.ValidateAndThrowAsync(criteria, token);

        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToMatch(entities);
    }

    public async Task<bool> CreateAsync(CreateRole create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        await _entityValidator.ValidateAndThrowAsync(entity, token);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyRole modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.RoleId, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        await _entityValidator.ValidateAndThrowAsync(entity, token);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(Guid role, CancellationToken token)
        => await _writer.DeleteAsync(role, token);
}