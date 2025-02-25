using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tek.Contract.Engine;
using Tek.Service.Security;

namespace Tek.Api.Engine;

[ApiController]
[ApiExplorerSettings(GroupName = "Security: Identification")]
[Authorize]
public class OrganizationController : ControllerBase
{
    private readonly OrganizationService _organizationService;

    public OrganizationController(OrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    [Authorize(Endpoints.SecurityApi.Identification.Organization.Assert)]
    [HttpGet(Endpoints.SecurityApi.Identification.Organization.Assert)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> AssertAsync([FromRoute] Guid organization, CancellationToken token)
    {
        var exists = await _organizationService.AssertAsync(organization, token);

        return Ok(exists);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Organization.Fetch)]
    [HttpGet(Endpoints.SecurityApi.Identification.Organization.Fetch)]
    [ProducesResponseType(typeof(OrganizationModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<OrganizationModel>> FetchAsync([FromRoute] Guid organization, CancellationToken token)
    {
        var model = await _organizationService.FetchAsync(organization, token);

        if (model == null)
            return NotFound();

        return Ok(model);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Organization.Count)]
    [HttpGet(Endpoints.SecurityApi.Identification.Organization.Count)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> CountAsync([FromQuery] IOrganizationCriteria criteria, CancellationToken token)
    {
        var count = await _organizationService.CountAsync(criteria, token);

        return Ok(count);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Organization.Collect)]
    [HttpGet(Endpoints.SecurityApi.Identification.Organization.Collect)]
    [ProducesResponseType(typeof(IEnumerable<OrganizationModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OrganizationModel>>> CollectAsync([FromQuery] IOrganizationCriteria criteria, CancellationToken token)
    {
        var models = await _organizationService.CollectAsync(criteria, token);

        var count = await _organizationService.CountAsync(criteria, token);

        Response.AddPagination(criteria.Filter.Page, criteria.Filter.Take, models.Count(), count);

        return Ok(models);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Organization.Search)]
    [HttpGet(Endpoints.SecurityApi.Identification.Organization.Search)]
    [ProducesResponseType(typeof(IEnumerable<OrganizationMatch>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OrganizationMatch>>> SearchAsync([FromQuery] IOrganizationCriteria criteria, CancellationToken token)
    {
        var matches = await _organizationService.SearchAsync(criteria, token);

        var count = await _organizationService.CountAsync(criteria, token);

        Response.AddPagination(criteria.Filter.Page, criteria.Filter.Take, matches.Count(), count);

        return Ok(matches);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Organization.Create)]
    [HttpPost(Endpoints.SecurityApi.Identification.Organization.Create)]
    [ProducesResponseType(typeof(OrganizationModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrganizationModel>> CreateAsync([FromBody] CreateOrganization create, CancellationToken token)
    {
        var created = await _organizationService.CreateAsync(create, token);

        if (!created)
            return BadRequest($"Duplicate not permitted: OrganizationId {create.OrganizationId}. You cannot insert a duplicate object with the same primary key.");

        var model = await _organizationService.FetchAsync(create.OrganizationId, token);

        return CreatedAtAction(nameof(CreateAsync), model);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Organization.Modify)]
    [HttpPut(Endpoints.SecurityApi.Identification.Organization.Modify)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ModifyAsync([FromBody] ModifyOrganization modify, CancellationToken token)
    {
        var model = await _organizationService.FetchAsync(modify.OrganizationId, token);

        if (model is null)
            return NotFound($"Organization not found: OrganizationId {modify.OrganizationId}. You cannot modify an object that is not in the database.");

        var modified = await _organizationService.ModifyAsync(modify, token);

        if (!modified)
            return NotFound();
        
        return Ok();
    }

    [Authorize(Endpoints.SecurityApi.Identification.Organization.Delete)]
    [HttpDelete(Endpoints.SecurityApi.Identification.Organization.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid organization, CancellationToken token)
    {
        var deleted = await _organizationService.DeleteAsync(organization, token);
        
        if (!deleted)
            return NotFound();
        
        return Ok();
    }
}