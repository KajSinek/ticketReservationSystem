using Hangfire;
using Helpers.Responses;
using Microsoft.AspNetCore.Mvc;

public class HomeTestingController : Controller
{
    public IActionResult Index()
    {
        // Enqueue a background job
        BackgroundJob.Enqueue(() => Console.WriteLine("Hello, Hangfire!"));
        return View();
    }

    [HttpPost]
    [Route("api/backgroundJobTest")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EntityResponse<int>))]
    public async Task<IActionResult> CreateAccountAsync(
        CancellationToken ct
    )
    {
        BackgroundJob.Enqueue(() => Console.WriteLine("Hello, Hangfire!"));
        var response = new EntityResponse<int> { Entity = 1 };
        return Ok(response);
    }

    [HttpGet]
    [Route("api/backgroundJobTest")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EntityResponse<string>))]
    public async Task<IActionResult> GetDataAsync(
        CancellationToken ct
    )
    {
        BackgroundJob.Enqueue(() => Console.WriteLine("Hello, Hangfire!"));
        var response = new EntityResponse<string> { Entity = "Hello, Hangfire!" };
        return Ok(response);
    }
}
