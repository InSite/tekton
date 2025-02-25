using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Contact;

public class ProvinceService
{
    private readonly TProvinceReader _reader;
    private readonly TProvinceWriter _writer;

    private readonly ProvinceAdapter _adapter = new ProvinceAdapter();

    private readonly IValidator<IProvinceCriteria> _criteriaValidator;
    private readonly IValidator<TProvinceEntity> _entityValidator;

    public ProvinceService(TProvinceReader reader, TProvinceWriter writer,
        IValidator<IProvinceCriteria> criteriaValidator, IValidator<TProvinceEntity> entityValidator)
    {
        _reader = reader;
        _writer = writer;

        _criteriaValidator = criteriaValidator;
        _entityValidator = entityValidator;
    }

    public async Task<bool> AssertAsync(Guid province, CancellationToken token)
        => await _reader.AssertAsync(province, token);

    public async Task<int> CountAsync(IProvinceCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<ProvinceModel?> FetchAsync(Guid province, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(province, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<IEnumerable<ProvinceModel>> CollectAsync(IProvinceCriteria criteria, CancellationToken token)
    {
        await _criteriaValidator.ValidateAndThrowAsync(criteria, token);

        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<ProvinceMatch>> SearchAsync(IProvinceCriteria criteria, CancellationToken token)
    {
        await _criteriaValidator.ValidateAndThrowAsync(criteria, token);

        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToMatch(entities);
    }

    public async Task<bool> CreateAsync(CreateProvince create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        await _entityValidator.ValidateAndThrowAsync(entity, token);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyProvince modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.ProvinceId, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        await _entityValidator.ValidateAndThrowAsync(entity, token);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(Guid province, CancellationToken token)
        => await _writer.DeleteAsync(province, token);
}