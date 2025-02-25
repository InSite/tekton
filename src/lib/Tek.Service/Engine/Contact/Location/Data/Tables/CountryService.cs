using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Contact;

public class CountryService
{
    private readonly TCountryReader _reader;
    private readonly TCountryWriter _writer;

    private readonly CountryAdapter _adapter = new CountryAdapter();

    private readonly IValidator<ICountryCriteria> _criteriaValidator;
    private readonly IValidator<TCountryEntity> _entityValidator;

    public CountryService(TCountryReader reader, TCountryWriter writer,
        IValidator<ICountryCriteria> criteriaValidator, IValidator<TCountryEntity> entityValidator)
    {
        _reader = reader;
        _writer = writer;

        _criteriaValidator = criteriaValidator;
        _entityValidator = entityValidator;
    }

    public async Task<bool> AssertAsync(Guid country, CancellationToken token)
        => await _reader.AssertAsync(country, token);

    public async Task<int> CountAsync(ICountryCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<CountryModel?> FetchAsync(Guid country, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(country, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<IEnumerable<CountryModel>> CollectAsync(ICountryCriteria criteria, CancellationToken token)
    {
        await _criteriaValidator.ValidateAndThrowAsync(criteria, token);

        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<CountryMatch>> SearchAsync(ICountryCriteria criteria, CancellationToken token)
    {
        await _criteriaValidator.ValidateAndThrowAsync(criteria, token);

        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToMatch(entities);
    }

    public async Task<bool> CreateAsync(CreateCountry create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        await _entityValidator.ValidateAndThrowAsync(entity, token);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyCountry modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.CountryId, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        await _entityValidator.ValidateAndThrowAsync(entity, token);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(Guid country, CancellationToken token)
        => await _writer.DeleteAsync(country, token);
}