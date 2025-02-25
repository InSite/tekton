﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tek.Api;

[ApiController]
[ApiExplorerSettings(GroupName = "Security: Debug")]
public class DebugController : ControllerBase
{
    private readonly IClaimConverter _claimConverter;
    private readonly Authorizer _authorizer;

    public DebugController(IClaimConverter principalAdapter, Authorizer authorizer)
    {
        _authorizer = authorizer;
        _claimConverter = principalAdapter;
    }

    [HttpGet(CoreEndpoints.Debug.Endpoints)]
    [Authorize(CoreEndpoints.Debug.Endpoints)]
    [ApiExplorerSettings(GroupName = "Security: Debug")]
    public IActionResult DebugPaths()
    {
        var reflector = new Reflector();

        var paths = reflector.FindRelativeUrls(typeof(CoreEndpoints))
            .OrderBy(x => x.Key)
            .ToDictionary(x => x.Key, x => x.Value);

        return Ok(paths);
    }

    [HttpGet(CoreEndpoints.Debug.Permissions)]
    [Authorize(CoreEndpoints.Debug.Permissions)]
    [ApiExplorerSettings(GroupName = "Security: Debug")]
    public IActionResult DebugPermissions()
    {
        var result = new
        {
            _authorizer.Domain,
            _authorizer.NamespaceId,
            Permissions = _authorizer.GetPermissions()
        };

        return Ok(result);
    }

    [HttpGet(CoreEndpoints.Debug.Resources)]
    [Authorize(CoreEndpoints.Debug.Resources)]
    [ApiExplorerSettings(GroupName = "Security: Debug")]
    public IActionResult DebugResources()
    {
        var result = new
        {
            _authorizer.Domain,
            _authorizer.NamespaceId,
            Resources = _authorizer.GetResources()
        };

        return Ok(result);
    }

    [HttpGet(CoreEndpoints.Debug.Token)]
    [Authorize(CoreEndpoints.Debug.Token)]
    [ApiExplorerSettings(GroupName = "Security: Debug")]
    public IActionResult DebugToken()
    {
        var encoder = new JwtEncoder();

        var token = encoder.Extract(JwtAuthenticationOptions.DefaultScheme, Request.Headers["Authorization"]);

        var jwt = encoder.Decode(token);

        var principal = _claimConverter.ToPrincipal(jwt);

        var result = new
        {
            Jwt = jwt,
            Principal = principal
        };

        return Ok(result);
    }
}