using System.Net;
using System.Net.Mime;
using AutomaticBroccoli.API.Contracts;
using AutomaticBroccoli.DataAccess.Postgres;
using AutomaticBroccoli.DataAccess.Postgres.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Npgsql;

namespace AutomaticBroccoli.API.Controllers;

[ApiController]
[Route("v2/openLoops")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class OpenLoopsControllerV2 : ControllerBase
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
    public async Task<IActionResult> GetV2([FromQuery]string? note)
    {
        var openLoops = await _context.OpenLoops
            .ToArrayAsync();

        // var users = await _context.Users.ToArrayAsync();
        // var userIds = users.Select(x => x.Id).ToHashSet();

        // var o = await _context.OpenLoops
        //     .Where(x => userIds.Contains(x.UserId))
        //     .ToArrayAsync();

        var userId = 1;

        var userNotes1 = await
            (from user in _context.Users
             join openLoop in _context.OpenLoops
                 on user.Id equals openLoop.UserId into grouping
             where user.Id == userId
             from un in grouping.DefaultIfEmpty()
             select new
             {
                 Login = user.Login,
                 Note = un.Note
             })
            .ToArrayAsync();

        var openLoops1 = await _context.Users
            .Where(x => x.Id == userId)
            .GroupJoin(_context.OpenLoops,
                x => x.Id,
                x => x.UserId,
                (u, o) => new { u.Login, o })
            .SelectMany(
                x => x.o.DefaultIfEmpty(),
                (u, o) => new { Login = u.Login, Note = o.Note })
            .ToArrayAsync();

        FormattableString sql = @$"
            SELECT u.""Login"", o.""Note"" FROM ""Users"" u
            LEFT JOIN ""OpenLoops"" o ON o.""UserId"" = u.""Id""
            WHERE o.""Note"" = {note}";

        var userNotes2 = await _context.Set<UserNotes>()
            .FromSqlInterpolated(sql)
            .ToArrayAsync();

        // var openLoops1 = await _context.OpenLoops
        //     .AsNoTracking()
        //     .Include(x => x.User)
        //     .ToArrayAsync();

        // var openLoops2 = await _context.OpenLoops
        //     .ToArrayAsync();

        // await _context.Entry(openLoops2.First())
        //     .Reference(x => x.User)
        //     .LoadAsync();


        var response = new GetOpenLoopsResponse
        {
            OpenLoops = openLoops.Select(x => new GetOpenLoopDto
            {
                Id = x.Id,
                Note = x.Note,
                CreatedDate = x.CreatedDate,
                UserLogin = x.User?.Login
            }).ToArray()
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