using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Tek.Contract.Engine;

namespace Tek.Service.Bus;

public class EventService
{
    private readonly TEventReader _reader;
    private readonly TEventWriter _writer;

    private readonly EventAdapter _adapter = new EventAdapter();

    private readonly IValidator<IEventCriteria> _criteriaValidator;
    private readonly IValidator<TEventEntity> _entityValidator;

    public EventService(TEventReader reader, TEventWriter writer,
        IValidator<IEventCriteria> criteriaValidator, IValidator<TEventEntity> entityValidator)
    {
        _reader = reader;
        _writer = writer;

        _criteriaValidator = criteriaValidator;
        _entityValidator = entityValidator;
    }

    public async Task<bool> AssertAsync(Guid @event, CancellationToken token)
        => await _reader.AssertAsync(@event, token);

    public async Task<int> CountAsync(IEventCriteria criteria, CancellationToken token)
        => await _reader.CountAsync(criteria, token);

    public async Task<EventModel?> FetchAsync(Guid @event, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(@event, token);

        return entity != null ? _adapter.ToModel(entity) : null;
    }

    public async Task<IEnumerable<EventModel>> CollectAsync(IEventCriteria criteria, CancellationToken token)
    {
        await _criteriaValidator.ValidateAndThrowAsync(criteria, token);

        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToModel(entities);
    }

    public async Task<IEnumerable<EventMatch>> SearchAsync(IEventCriteria criteria, CancellationToken token)
    {
        await _criteriaValidator.ValidateAndThrowAsync(criteria, token);

        var entities = await _reader.CollectAsync(criteria, token);

        return _adapter.ToMatch(entities);
    }

    public async Task<bool> CreateAsync(CreateEvent create, CancellationToken token)
    {
        var entity = _adapter.ToEntity(create);

        await _entityValidator.ValidateAndThrowAsync(entity, token);

        return await _writer.CreateAsync(entity, token);
    }

    public async Task<bool> ModifyAsync(ModifyEvent modify, CancellationToken token)
    {
        var entity = await _reader.FetchAsync(modify.EventId, token);

        if (entity == null)
            return false;

        _adapter.Copy(modify, entity);

        await _entityValidator.ValidateAndThrowAsync(entity, token);

        return await _writer.ModifyAsync(entity, token);
    }

    public async Task<bool> DeleteAsync(Guid @event, CancellationToken token)
        => await _writer.DeleteAsync(@event, token);
}