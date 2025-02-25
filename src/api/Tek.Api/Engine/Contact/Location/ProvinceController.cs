using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Tek.Contract.Engine;
using Tek.Service.Contact;

namespace Tek.Api.Engine;

[ApiController]
[ApiExplorerSettings(GroupName = "Contact: Location")]
[Authorize]
public class ProvinceController : ControllerBase
{
    private readonly ProvinceService _provinceService;

    public ProvinceController(ProvinceService provinceService)
    {
        _provinceService = provinceService;
    }

    [Authorize(Endpoints.ContactApi.Location.Province.Assert)]
    [HttpGet(Endpoints.ContactApi.Location.Province.Assert)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> AssertAsync([FromRoute] Guid province, CancellationToken token)
    {
        var exists = await _provinceService.AssertAsync(province, token);

        return Ok(exists);
    }

    [Authorize(Endpoints.ContactApi.Location.Province.Fetch)]
    [HttpGet(Endpoints.ContactApi.Location.Province.Fetch)]
    [ProducesResponseType(typeof(ProvinceModel), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProvinceModel>> FetchAsync([FromRoute] Guid province, CancellationToken token)
    {
        var model = await _provinceService.FetchAsync(province, token);

        if (model == null)
            return NotFound();

        return Ok(model);
    }

    [Authorize(Endpoints.ContactApi.Location.Province.Count)]
    [HttpGet(Endpoints.ContactApi.Location.Province.Count)]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> CountAsync([FromQuery] IProvinceCriteria criteria, CancellationToken token)
    {
        var count = await _provinceService.CountAsync(criteria, token);

        return Ok(count);
    }

    [Authorize(Endpoints.ContactApi.Location.Province.Collect)]
    [HttpGet(Endpoints.ContactApi.Location.Province.Collect)]
    [ProducesResponseType(typeof(IEnumerable<ProvinceModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProvinceModel>>> CollectAsync([FromQuery] IProvinceCriteria criteria, CancellationToken token)
    {
        var models = await _provinceService.CollectAsync(criteria, token);

        var count = await _provinceService.CountAsync(criteria, token);

        Response.AddPagination(criteria.Filter.Page, criteria.Filter.Take, models.Count(), count);

        return Ok(models);
    }

    [Authorize(Endpoints.ContactApi.Location.Province.Search)]
    [HttpGet(Endpoints.ContactApi.Location.Province.Search)]
    [ProducesResponseType(typeof(IEnumerable<ProvinceMatch>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProvinceMatch>>> SearchAsync([FromQuery] IProvinceCriteria criteria, CancellationToken token)
    {
        var matches = await _provinceService.SearchAsync(criteria, token);

        var count = await _provinceService.CountAsync(criteria, token);

        Response.AddPagination(criteria.Filter.Page, criteria.Filter.Take, matches.Count(), count);

        return Ok(matches);
    }

    [Authorize(Endpoints.ContactApi.Location.Province.Create)]
    [HttpPost(Endpoints.ContactApi.Location.Province.Create)]
    [ProducesResponseType(typeof(ProvinceModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProvinceModel>> CreateAsync([FromBody] CreateProvince create, CancellationToken token)
    {
        var created = await _provinceService.CreateAsync(create, token);

        if (!created)
            return BadRequest($"Duplicate not permitted: ProvinceId {create.ProvinceId}. You cannot insert a duplicate object with the same primary key.");

        var model = await _provinceService.FetchAsync(create.ProvinceId, token);

        return CreatedAtAction(nameof(CreateAsync), model);
    }

    [Authorize(Endpoints.ContactApi.Location.Province.Modify)]
    [HttpPut(Endpoints.ContactApi.Location.Province.Modify)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationFailure), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ModifyAsync([FromBody] ModifyProvince modify, CancellationToken token)
    {
        var model = await _provinceService.FetchAsync(modify.ProvinceId, token);

        if (model is null)
            return NotFound($"Province not found: ProvinceId {modify.ProvinceId}. You cannot modify an object that is not in the database.");

        var modified = await _provinceService.ModifyAsync(modify, token);

        if (!modified)
            return NotFound();
        
        return Ok();
    }

    [Authorize(Endpoints.ContactApi.Location.Province.Delete)]
    [HttpDelete(Endpoints.ContactApi.Location.Province.Delete)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid province, CancellationToken token)
    {
        var deleted = await _provinceService.DeleteAsync(province, token);
        
        if (!deleted)
            return NotFound();
        
        return Ok();
    }
}