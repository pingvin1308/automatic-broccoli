using System.Net;
using AutomaticBroccoli.API.Contracts;
using AutomaticBroccoli.CLI;
using AutomaticBroccoli.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace AutomaticBroccoli.API.Controllers;

public class OpenLoopsController : BaseController
{
    private readonly ILogger<OpenLoopsController> _logger;

    public OpenLoopsController(ILogger<OpenLoopsController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get list of open loops.
    /// </summary>
    /// <returns>List of open loops.</returns>
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
    public async Task<IActionResult> Create([FromBody] CreateOpenLoopRequest request)
    {
        var openLoop = new OpenLoop(Guid.NewGuid(), request.Note, DateTimeOffset.UtcNow);
        var openLoopId = OpenLoopsRepository.Add(openLoop);
        return Ok(openLoopId);
    }
}