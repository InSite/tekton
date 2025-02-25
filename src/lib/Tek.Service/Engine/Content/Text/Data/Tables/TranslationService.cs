using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Content;

public class TranslationService
{
    private readonly TTranslationReader _reader;
    private readonly TTranslationWriter _writer;

    private readonly TranslationAdapter _adapter = new TranslationAdapter();

    private readonly IValidator<ITranslationCriteria> _criteriaValidator;
    private readonly IValidator<TTranslationEntity> _entityValidator;

    public TranslationService(TTranslationReader reader, TTranslationWriter writer,
        IValidator<ITranslationCriteria> criteriaValidator, IValidator<TTranslationEntity> entityValidator)
    {
        _reader = reader;
        _writer = writer;

        _criteriaValidator = criteriaValidator;
        _entityValidator = entityValidator;
    }

    public async Task<bool> AssertAsync(Guid translation, CancellationToken token)
        => await _reader.AssertAsync(translation, token);

    public async Task<int> CountAsync(ITranslationCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<TranslationModel?> FetchAsync(Guid translation, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(translation, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<IEnumerable<TranslationModel>> CollectAsync(ITranslationCriteria criteria, CancellationToken token)
    {
        await _criteriaValidator.ValidateAndThrowAsync(criteria, token);

        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<TranslationMatch>> SearchAsync(ITranslationCriteria criteria, CancellationToken token)
    {
        await _criteriaValidator.ValidateAndThrowAsync(criteria, token);

        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToMatch(entities);
    }

    public async Task<bool> CreateAsync(CreateTranslation create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        await _entityValidator.ValidateAndThrowAsync(entity, token);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyTranslation modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.TranslationId, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        await _entityValidator.ValidateAndThrowAsync(entity, token);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(Guid translation, CancellationToken token)
        => await _writer.DeleteAsync(translation, token);
}