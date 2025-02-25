using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tek.Contract.Engine;
using Tek.Service.Security;

namespace Tek.Api.Engine;

[ApiController]
[ApiExplorerSettings(GroupName = "Security: Identification")]
[Authorize]
public class PasswordController : ControllerBase
{
    private readonly PasswordService _passwordService;

    public PasswordController(PasswordService passwordService)
    {
        _passwordService = passwordService;
    }

    [Authorize(Endpoints.SecurityApi.Identification.Password.Assert)]
    [HttpGet(Endpoints.SecurityApi.Identification.Password.Assert)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> AssertAsync([FromRoute] Guid password, CancellationToken token)
    {
        var exists = await _passwordService.AssertAsync(password, token);

        return Ok(exists);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Password.Fetch)]
    [HttpGet(Endpoints.SecurityApi.Identification.Password.Fetch)]
    [ProducesResponseType(typeof(PasswordModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<PasswordModel>> FetchAsync([FromRoute] Guid password, CancellationToken token)
    {
        var model = await _passwordService.FetchAsync(password, token);

        if (model == null)
            return NotFound();

        return Ok(model);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Password.Count)]
    [HttpGet(Endpoints.SecurityApi.Identification.Password.Count)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> CountAsync([FromQuery] IPasswordCriteria criteria, CancellationToken token)
    {
        var count = await _passwordService.CountAsync(criteria, token);

        return Ok(count);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Password.Collect)]
    [HttpGet(Endpoints.SecurityApi.Identification.Password.Collect)]
    [ProducesResponseType(typeof(IEnumerable<PasswordModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PasswordModel>>> CollectAsync([FromQuery] IPasswordCriteria criteria, CancellationToken token)
    {
        var models = await _passwordService.CollectAsync(criteria, token);

        var count = await _passwordService.CountAsync(criteria, token);

        Response.AddPagination(criteria.Filter.Page, criteria.Filter.Take, models.Count(), count);

        return Ok(models);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Password.Search)]
    [HttpGet(Endpoints.SecurityApi.Identification.Password.Search)]
    [ProducesResponseType(typeof(IEnumerable<PasswordMatch>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PasswordMatch>>> SearchAsync([FromQuery] IPasswordCriteria criteria, CancellationToken token)
    {
        var matches = await _passwordService.SearchAsync(criteria, token);

        var count = await _passwordService.CountAsync(criteria, token);

        Response.AddPagination(criteria.Filter.Page, criteria.Filter.Take, matches.Count(), count);

        return Ok(matches);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Password.Create)]
    [HttpPost(Endpoints.SecurityApi.Identification.Password.Create)]
    [ProducesResponseType(typeof(PasswordModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PasswordModel>> CreateAsync([FromBody] CreatePassword create, CancellationToken token)
    {
        var created = await _passwordService.CreateAsync(create, token);

        if (!created)
            return BadRequest($"Duplicate not permitted: PasswordId {create.PasswordId}. You cannot insert a duplicate object with the same primary key.");

        var model = await _passwordService.FetchAsync(create.PasswordId, token);

        return CreatedAtAction(nameof(CreateAsync), model);
    }

    [Authorize(Endpoints.SecurityApi.Identification.Password.Modify)]
    [HttpPut(Endpoints.SecurityApi.Identification.Password.Modify)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ModifyAsync([FromBody] ModifyPassword modify, CancellationToken token)
    {
        var model = await _passwordService.FetchAsync(modify.PasswordId, token);

        if (model is null)
            return NotFound($"Password not found: PasswordId {modify.PasswordId}. You cannot modify an object that is not in the database.");

        var modified = await _passwordService.ModifyAsync(modify, token);

        if (!modified)
            return NotFound();
        
        return Ok();
    }

    [Authorize(Endpoints.SecurityApi.Identification.Password.Delete)]
    [HttpDelete(Endpoints.SecurityApi.Identification.Password.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid password, CancellationToken token)
    {
        var deleted = await _passwordService.DeleteAsync(password, token);
        
        if (!deleted)
            return NotFound();
        
        return Ok();
    }
}