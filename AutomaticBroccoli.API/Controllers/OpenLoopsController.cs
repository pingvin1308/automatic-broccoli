using System.Net;
using System.Net.Mime;
using AutomaticBroccoli.API.Contracts;
using AutomaticBroccoli.CLI;
using AutomaticBroccoli.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace AutomaticBroccoli.API.Controllers;

[ApiController]
[Route("v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class OpenLoopsController : ControllerBase
{
    private readonly ILogger<OpenLoopsController> _logger;

    public OpenLoopsController(ILogger<OpenLoopsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetOpenLoopsResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get()
    {
        var openLoops = OpenLoopsRepository.Get();

        var response = new GetOpenLoopsResponse
        {
            OpenLoops = openLoops.Select(x => new GetOpenLoopDto
            {
                Id = x.Id,
                Note = x.Note,
                CreatedDate = x.CreatedDate
            }).ToArray()
        };

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Create([FromBody]CreateOpenLoopRequest request)
    {
        var openLoop = new OpenLoop(Guid.NewGuid(), request.Note, DateTimeOffset.UtcNow);
        var openLoopId = OpenLoopsRepository.Add(openLoop);
        return Ok(openLoopId);
    }
}