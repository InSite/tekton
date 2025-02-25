using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tek.Contract.Engine;
using Tek.Service.Metadata;

namespace Tek.Api.Engine;

[ApiController]
[ApiExplorerSettings(GroupName = "Metadata: Audit")]
[Authorize]
public class VersionController : ControllerBase
{
    private readonly VersionService _versionService;

    public VersionController(VersionService versionService)
    {
        _versionService = versionService;
    }

    [Authorize(Endpoints.MetadataApi.Audit.Version.Assert)]
    [HttpGet(Endpoints.MetadataApi.Audit.Version.Assert)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> AssertAsync([FromRoute] int versionNumber, CancellationToken token)
    {
        var exists = await _versionService.AssertAsync(versionNumber, token);

        return Ok(exists);
    }

    [Authorize(Endpoints.MetadataApi.Audit.Version.Fetch)]
    [HttpGet(Endpoints.MetadataApi.Audit.Version.Fetch)]
    [ProducesResponseType(typeof(VersionModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<VersionModel>> FetchAsync([FromRoute] int versionNumber, CancellationToken token)
    {
        var model = await _versionService.FetchAsync(versionNumber, token);

        if (model == null)
            return NotFound();

        return Ok(model);
    }

    [Authorize(Endpoints.MetadataApi.Audit.Version.Count)]
    [HttpGet(Endpoints.MetadataApi.Audit.Version.Count)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> CountAsync([FromQuery] IVersionCriteria criteria, CancellationToken token)
    {
        var count = await _versionService.CountAsync(criteria, token);

        return Ok(count);
    }

    [Authorize(Endpoints.MetadataApi.Audit.Version.Collect)]
    [HttpGet(Endpoints.MetadataApi.Audit.Version.Collect)]
    [ProducesResponseType(typeof(IEnumerable<VersionModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VersionModel>>> CollectAsync([FromQuery] IVersionCriteria criteria, CancellationToken token)
    {
        var models = await _versionService.CollectAsync(criteria, token);

        var count = await _versionService.CountAsync(criteria, token);

        Response.AddPagination(criteria.Filter.Page, criteria.Filter.Take, models.Count(), count);

        return Ok(models);
    }

    [Authorize(Endpoints.MetadataApi.Audit.Version.Search)]
    [HttpGet(Endpoints.MetadataApi.Audit.Version.Search)]
    [ProducesResponseType(typeof(IEnumerable<VersionMatch>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VersionMatch>>> SearchAsync([FromQuery] IVersionCriteria criteria, CancellationToken token)
    {
        var matches = await _versionService.SearchAsync(criteria, token);

        var count = await _versionService.CountAsync(criteria, token);

        Response.AddPagination(criteria.Filter.Page, criteria.Filter.Take, matches.Count(), count);

        return Ok(matches);
    }

    [Authorize(Endpoints.MetadataApi.Audit.Version.Create)]
    [HttpPost(Endpoints.MetadataApi.Audit.Version.Create)]
    [ProducesResponseType(typeof(VersionModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VersionModel>> CreateAsync([FromBody] CreateVersion create, CancellationToken token)
    {
        var created = await _versionService.CreateAsync(create, token);

        if (!created)
            return BadRequest($"Duplicate not permitted: VersionNumber {create.VersionNumber}. You cannot insert a duplicate object with the same primary key.");

        var model = await _versionService.FetchAsync(create.VersionNumber, token);

        return CreatedAtAction(nameof(CreateAsync), model);
    }

    [Authorize(Endpoints.MetadataApi.Audit.Version.Modify)]
    [HttpPut(Endpoints.MetadataApi.Audit.Version.Modify)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ModifyAsync([FromBody] ModifyVersion modify, CancellationToken token)
    {
        var model = await _versionService.FetchAsync(modify.VersionNumber, token);

        if (model is null)
            return NotFound($"Version not found: VersionNumber {modify.VersionNumber}. You cannot modify an object that is not in the database.");

        var modified = await _versionService.ModifyAsync(modify, token);

        if (!modified)
            return NotFound();
        
        return Ok();
    }

    [Authorize(Endpoints.MetadataApi.Audit.Version.Delete)]
    [HttpDelete(Endpoints.MetadataApi.Audit.Version.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int versionNumber, CancellationToken token)
    {
        var deleted = await _versionService.DeleteAsync(versionNumber, token);
        
        if (!deleted)
            return NotFound();
        
        return Ok();
    }
}