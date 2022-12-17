using Microsoft.AspNetCore.Mvc;

namespace AutomaticBroccoli.API.Controllers;

public class ClarificationController : BaseController
{
    /// <summary>
    /// Get list of open loops that need to be clarified.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject()
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAction()
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateMaterial()
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> MoveToTrash()
    {
        return Ok();
    }
}