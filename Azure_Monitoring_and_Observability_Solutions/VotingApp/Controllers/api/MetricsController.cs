using Microsoft.AspNetCore.Mvc;

namespace VotingApp.Controllers.api;

[Route("api/[controller]")]
[ApiController]
public class MetricsController : Controller
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    private readonly ILogger<MetricsController> _logger;

    
    public MetricsController(ILogger<MetricsController> logger)
    {
        _logger = logger;                
    }
        
}