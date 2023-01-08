using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace AutomaticBroccoli.API.Controllers;

/// <inheritdoc></inheritdoc>
[ApiController]
[Route("v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public abstract class BaseController : ControllerBase
{
}
