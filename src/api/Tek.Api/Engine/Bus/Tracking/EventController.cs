using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tek.Contract.Engine;
using Tek.Service.Bus;

namespace Tek.Api.Engine;

[ApiController]
[ApiExplorerSettings(GroupName = "Bus: Tracking")]
[Authorize]
public class EventController : ControllerBase
{
    private readonly EventService _eventService;

    public EventController(EventService eventService)
    {
        _eventService = eventService;
    }

    [Authorize(Endpoints.BusApi.Tracking.Event.Assert)]
    [HttpGet(Endpoints.BusApi.Tracking.Event.Assert)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> AssertAsync([FromRoute] Guid @event, CancellationToken token)
    {
        var exists = await _eventService.AssertAsync(@event, token);

        return Ok(exists);
    }

    [Authorize(Endpoints.BusApi.Tracking.Event.Fetch)]
    [HttpGet(Endpoints.BusApi.Tracking.Event.Fetch)]
    [ProducesResponseType(typeof(EventModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<EventModel>> FetchAsync([FromRoute] Guid @event, CancellationToken token)
    {
        var model = await _eventService.FetchAsync(@event, token);

        if (model == null)
            return NotFound();

        return Ok(model);
    }

    [Authorize(Endpoints.BusApi.Tracking.Event.Count)]
    [HttpGet(Endpoints.BusApi.Tracking.Event.Count)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> CountAsync([FromQuery] IEventCriteria criteria, CancellationToken token)
    {
        var count = await _eventService.CountAsync(criteria, token);

        return Ok(count);
    }

    [Authorize(Endpoints.BusApi.Tracking.Event.Collect)]
    [HttpGet(Endpoints.BusApi.Tracking.Event.Collect)]
    [ProducesResponseType(typeof(IEnumerable<EventModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EventModel>>> CollectAsync([FromQuery] IEventCriteria criteria, CancellationToken token)
    {
        var models = await _eventService.CollectAsync(criteria, token);

        var count = await _eventService.CountAsync(criteria, token);

        Response.AddPagination(criteria.Filter.Page, criteria.Filter.Take, models.Count(), count);

        return Ok(models);
    }

    [Authorize(Endpoints.BusApi.Tracking.Event.Search)]
    [HttpGet(Endpoints.BusApi.Tracking.Event.Search)]
    [ProducesResponseType(typeof(IEnumerable<EventMatch>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<EventMatch>>> SearchAsync([FromQuery] IEventCriteria criteria, CancellationToken token)
    {
        var matches = await _eventService.SearchAsync(criteria, token);

        var count = await _eventService.CountAsync(criteria, token);

        Response.AddPagination(criteria.Filter.Page, criteria.Filter.Take, matches.Count(), count);

        return Ok(matches);
    }

    [Authorize(Endpoints.BusApi.Tracking.Event.Create)]
    [HttpPost(Endpoints.BusApi.Tracking.Event.Create)]
    [ProducesResponseType(typeof(EventModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EventModel>> CreateAsync([FromBody] CreateEvent create, CancellationToken token)
    {
        var created = await _eventService.CreateAsync(create, token);

        if (!created)
            return BadRequest($"Duplicate not permitted: EventId {create.EventId}. You cannot insert a duplicate object with the same primary key.");

        var model = await _eventService.FetchAsync(create.EventId, token);

        return CreatedAtAction(nameof(CreateAsync), model);
    }

    [Authorize(Endpoints.BusApi.Tracking.Event.Modify)]
    [HttpPut(Endpoints.BusApi.Tracking.Event.Modify)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ModifyAsync([FromBody] ModifyEvent modify, CancellationToken token)
    {
        var model = await _eventService.FetchAsync(modify.EventId, token);

        if (model is null)
            return NotFound($"Event not found: EventId {modify.EventId}. You cannot modify an object that is not in the database.");

        var modified = await _eventService.ModifyAsync(modify, token);

        if (!modified)
            return NotFound();
        
        return Ok();
    }

    [Authorize(Endpoints.BusApi.Tracking.Event.Delete)]
    [HttpDelete(Endpoints.BusApi.Tracking.Event.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid @event, CancellationToken token)
    {
        var deleted = await _eventService.DeleteAsync(@event, token);
        
        if (!deleted)
            return NotFound();
        
        return Ok();
    }
}