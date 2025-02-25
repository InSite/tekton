using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tek.Contract.Engine;
using Tek.Service.Bus;

namespace Tek.Api.Engine;

[ApiController]
[ApiExplorerSettings(GroupName = "Bus: Tracking")]
[Authorize]
public class AggregateController : ControllerBase
{
    private readonly AggregateService _aggregateService;

    public AggregateController(AggregateService aggregateService)
    {
        _aggregateService = aggregateService;
    }

    [Authorize(Endpoints.BusApi.Tracking.Aggregate.Assert)]
    [HttpGet(Endpoints.BusApi.Tracking.Aggregate.Assert)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> AssertAsync([FromRoute] Guid aggregate, CancellationToken token)
    {
        var exists = await _aggregateService.AssertAsync(aggregate, token);

        return Ok(exists);
    }

    [Authorize(Endpoints.BusApi.Tracking.Aggregate.Fetch)]
    [HttpGet(Endpoints.BusApi.Tracking.Aggregate.Fetch)]
    [ProducesResponseType(typeof(AggregateModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<AggregateModel>> FetchAsync([FromRoute] Guid aggregate, CancellationToken token)
    {
        var model = await _aggregateService.FetchAsync(aggregate, token);

        if (model == null)
            return NotFound();

        return Ok(model);
    }

    [Authorize(Endpoints.BusApi.Tracking.Aggregate.Count)]
    [HttpGet(Endpoints.BusApi.Tracking.Aggregate.Count)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> CountAsync([FromQuery] IAggregateCriteria criteria, CancellationToken token)
    {
        var count = await _aggregateService.CountAsync(criteria, token);

        return Ok(count);
    }

    [Authorize(Endpoints.BusApi.Tracking.Aggregate.Collect)]
    [HttpGet(Endpoints.BusApi.Tracking.Aggregate.Collect)]
    [ProducesResponseType(typeof(IEnumerable<AggregateModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AggregateModel>>> CollectAsync([FromQuery] IAggregateCriteria criteria, CancellationToken token)
    {
        var models = await _aggregateService.CollectAsync(criteria, token);

        var count = await _aggregateService.CountAsync(criteria, token);

        Response.AddPagination(criteria.Filter.Page, criteria.Filter.Take, models.Count(), count);

        return Ok(models);
    }

    [Authorize(Endpoints.BusApi.Tracking.Aggregate.Search)]
    [HttpGet(Endpoints.BusApi.Tracking.Aggregate.Search)]
    [ProducesResponseType(typeof(IEnumerable<AggregateMatch>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AggregateMatch>>> SearchAsync([FromQuery] IAggregateCriteria criteria, CancellationToken token)
    {
        var matches = await _aggregateService.SearchAsync(criteria, token);

        var count = await _aggregateService.CountAsync(criteria, token);

        Response.AddPagination(criteria.Filter.Page, criteria.Filter.Take, matches.Count(), count);

        return Ok(matches);
    }

    [Authorize(Endpoints.BusApi.Tracking.Aggregate.Create)]
    [HttpPost(Endpoints.BusApi.Tracking.Aggregate.Create)]
    [ProducesResponseType(typeof(AggregateModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AggregateModel>> CreateAsync([FromBody] CreateAggregate create, CancellationToken token)
    {
        var created = await _aggregateService.CreateAsync(create, token);

        if (!created)
            return BadRequest($"Duplicate not permitted: AggregateId {create.AggregateId}. You cannot insert a duplicate object with the same primary key.");

        var model = await _aggregateService.FetchAsync(create.AggregateId, token);

        return CreatedAtAction(nameof(CreateAsync), model);
    }

    [Authorize(Endpoints.BusApi.Tracking.Aggregate.Modify)]
    [HttpPut(Endpoints.BusApi.Tracking.Aggregate.Modify)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ModifyAsync([FromBody] ModifyAggregate modify, CancellationToken token)
    {
        var model = await _aggregateService.FetchAsync(modify.AggregateId, token);

        if (model is null)
            return NotFound($"Aggregate not found: AggregateId {modify.AggregateId}. You cannot modify an object that is not in the database.");

        var modified = await _aggregateService.ModifyAsync(modify, token);

        if (!modified)
            return NotFound();
        
        return Ok();
    }

    [Authorize(Endpoints.BusApi.Tracking.Aggregate.Delete)]
    [HttpDelete(Endpoints.BusApi.Tracking.Aggregate.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid aggregate, CancellationToken token)
    {
        var deleted = await _aggregateService.DeleteAsync(aggregate, token);
        
        if (!deleted)
            return NotFound();
        
        return Ok();
    }
}