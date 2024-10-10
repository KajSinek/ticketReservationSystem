using Hangfire;
using Microsoft.AspNetCore.Mvc;

public class HomeTestingController : Controller
{
    public IActionResult Index()
    {
        // Enqueue a background job
        BackgroundJob.Enqueue(() => Console.WriteLine("Hello, Hangfire!"));
        return View();
    }
}
