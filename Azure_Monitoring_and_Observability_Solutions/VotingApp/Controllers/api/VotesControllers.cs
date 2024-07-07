using Microsoft.AspNetCore.Mvc;

namespace VotingApp.Controllers.api;

[Route("api/[controller]")]
[ApiController]
public class VotesController : Controller
{
    private readonly ILogger<VotesController> logger;

    public VotesController(ILogger<VotesController> logger)
    {
            this.logger = logger;
            
    }
}