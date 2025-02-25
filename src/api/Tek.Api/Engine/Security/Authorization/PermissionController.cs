using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tek.Contract.Engine;
using Tek.Service.Security;

namespace Tek.Api.Engine;

[ApiController]
[ApiExplorerSettings(GroupName = "Security: Authorization")]
[Authorize]
public class PermissionController : ControllerBase
{
    private readonly PermissionService _permissionService;

    public PermissionController(PermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    [Authorize(Endpoints.SecurityApi.Authorization.Permission.Assert)]
    [HttpGet(Endpoints.SecurityApi.Authorization.Permission.Assert)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> AssertAsync([FromRoute] Guid permission, CancellationToken token)
    {
        var exists = await _permissionService.AssertAsync(permission, token);

        return Ok(exists);
    }

    [Authorize(Endpoints.SecurityApi.Authorization.Permission.Fetch)]
    [HttpGet(Endpoints.SecurityApi.Authorization.Permission.Fetch)]
    [ProducesResponseType(typeof(PermissionModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<PermissionModel>> FetchAsync([FromRoute] Guid permission, CancellationToken token)
    {
        var model = await _permissionService.FetchAsync(permission, token);

        if (model == null)
            return NotFound();

        return Ok(model);
    }

    [Authorize(Endpoints.SecurityApi.Authorization.Permission.Count)]
    [HttpGet(Endpoints.SecurityApi.Authorization.Permission.Count)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> CountAsync([FromQuery] IPermissionCriteria criteria, CancellationToken token)
    {
        var count = await _permissionService.CountAsync(criteria, token);

        return Ok(count);
    }

    [Authorize(Endpoints.SecurityApi.Authorization.Permission.Collect)]
    [HttpGet(Endpoints.SecurityApi.Authorization.Permission.Collect)]
    [ProducesResponseType(typeof(IEnumerable<PermissionModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PermissionModel>>> CollectAsync([FromQuery] IPermissionCriteria criteria, CancellationToken token)
    {
        var models = await _permissionService.CollectAsync(criteria, token);

        var count = await _permissionService.CountAsync(criteria, token);

        Response.AddPagination(criteria.Filter.Page, criteria.Filter.Take, models.Count(), count);

        return Ok(models);
    }

    [Authorize(Endpoints.SecurityApi.Authorization.Permission.Search)]
    [HttpGet(Endpoints.SecurityApi.Authorization.Permission.Search)]
    [ProducesResponseType(typeof(IEnumerable<PermissionMatch>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PermissionMatch>>> SearchAsync([FromQuery] IPermissionCriteria criteria, CancellationToken token)
    {
        var matches = await _permissionService.SearchAsync(criteria, token);

        var count = await _permissionService.CountAsync(criteria, token);

        Response.AddPagination(criteria.Filter.Page, criteria.Filter.Take, matches.Count(), count);

        return Ok(matches);
    }

    [Authorize(Endpoints.SecurityApi.Authorization.Permission.Create)]
    [HttpPost(Endpoints.SecurityApi.Authorization.Permission.Create)]
    [ProducesResponseType(typeof(PermissionModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PermissionModel>> CreateAsync([FromBody] CreatePermission create, CancellationToken token)
    {
        var created = await _permissionService.CreateAsync(create, token);

        if (!created)
            return BadRequest($"Duplicate not permitted: PermissionId {create.PermissionId}. You cannot insert a duplicate object with the same primary key.");

        var model = await _permissionService.FetchAsync(create.PermissionId, token);

        return CreatedAtAction(nameof(CreateAsync), model);
    }

    [Authorize(Endpoints.SecurityApi.Authorization.Permission.Modify)]
    [HttpPut(Endpoints.SecurityApi.Authorization.Permission.Modify)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ModifyAsync([FromBody] ModifyPermission modify, CancellationToken token)
    {
        var model = await _permissionService.FetchAsync(modify.PermissionId, token);

        if (model is null)
            return NotFound($"Permission not found: PermissionId {modify.PermissionId}. You cannot modify an object that is not in the database.");

        var modified = await _permissionService.ModifyAsync(modify, token);

        if (!modified)
            return NotFound();
        
        return Ok();
    }

    [Authorize(Endpoints.SecurityApi.Authorization.Permission.Delete)]
    [HttpDelete(Endpoints.SecurityApi.Authorization.Permission.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid permission, CancellationToken token)
    {
        var deleted = await _permissionService.DeleteAsync(permission, token);
        
        if (!deleted)
            return NotFound();
        
        return Ok();
    }
}