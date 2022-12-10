using System.Net;
using AutomaticBroccoli.API.Contracts;
using AutomaticBroccoli.DataAccess.Postgres;
using AutomaticBroccoli.DataAccess.Postgres.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutomaticBroccoli.API.Controllers;

[Route("v2/openLoops")]
public class OpenLoopsControllerV2 : BaseController
{
    private readonly ILogger<OpenLoopsController> _logger;
    private readonly AutomaticBroccoliDbContext _context;

    public OpenLoopsControllerV2(
        ILogger<OpenLoopsController> logger,
        AutomaticBroccoliDbContext context)
    {
        _logger = logger;
        this._context = context;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetOpenLoopsResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetV2([FromQuery]int userId, int offset = 0, int count = 50)
    {
        var openLoops = await _context
            .OpenLoops
            .Include(x => x.User)
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Skip(offset)
            .Take(count)
            .ToArrayAsync();
        
        var totalCountOfOpenLoops = await _context
            .OpenLoops
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .CountAsync();

        var response = new GetOpenLoopsResponse
        {
            OpenLoops = openLoops
                .Select(x => new GetOpenLoopDto
                {
                    Id = x.Id,
                    Note = x.Note,
                    CreatedDate = x.CreatedDate,
                    UserLogin = x.User.Login
                })
                .ToArray(),
            Total = totalCountOfOpenLoops
        };

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateV2([FromBody] CreateOpenLoopRequest request)
    {
        var openLoop = new OpenLoop
        {
            Id = Guid.NewGuid(),
            Note = request.Note,
            CreatedDate = DateTimeOffset.UtcNow,
            UserId = 1
        };
        _context.OpenLoops.Add(openLoop);
        await _context.SaveChangesAsync();

        return Ok(openLoop.Id);
    }
}