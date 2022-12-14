using System.Net;
using System.Net.Mime;
using AutomaticBroccoli.API.Contracts;
using AutomaticBroccoli.DataAccess.Postgres;
using AutomaticBroccoli.DataAccess.Postgres.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutomaticBroccoli.API.Controllers;

/// <summary>
/// List of operations for CRUD open loops.
/// </summary>
[Route("inbox")]
public sealed class InboxController : BaseController
{
    private readonly AutomaticBroccoliDbContext _context;

    /// <summary>
    /// Initialize new InboxController for working with open loops.
    /// </summary>
    /// <param name="context">Instance of db context.</param>
    public InboxController(AutomaticBroccoliDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get list of open loops by specific user id.
    /// </summary>
    /// <param name="userId">User id.</param>
    /// <param name="offset">(Optional) represent how much items will be skipped. By default 0.</param>
    /// <param name="count">(Optional) represent how much items will be retrieved. By default 50.</param>
    /// <returns></returns>
    [HttpGet("openLoops")]
    [ProducesResponseType(typeof(GetOpenLoopsResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Get(
        [FromQuery] int userId,
        [FromQuery] int offset = 0,
        [FromQuery] int count = 50)
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

    /// <summary>
    /// Create new open loop.
    /// </summary>
    /// <param name="request">Request which contains information for creating new open loop.</param>
    /// <returns>Id of created open loop.</returns>
    [HttpPost("openLoops")]
    [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Create([FromBody] CreateOpenLoopRequest request)
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

    [HttpPost("attachments")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateAttachment(IFormFile file, [FromForm] string name)
    {
        var (fileName, attachmentId) = await Attachments.Create(file, name);
        var attachment = new Attachment
        {
            Id = attachmentId,
            Size = 0,
            Link = fileName,
            Name = fileName
        };
        _context.Attachments.Add(attachment);
        await _context.SaveChangesAsync();

        return Ok(attachment.Link);
    }

    [HttpGet("attachments")]
    public async Task<IActionResult> GetAttachment(Guid attachmentId)
    {
        var filePath = "";
        using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var memoryStream = new MemoryStream();
        return File(memoryStream.ToArray(), MediaTypeNames.Text.Plain, Guid.NewGuid().ToString());
    }
}
