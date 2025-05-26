using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConsumerComplaints.WebAPI.Controllers;

[ApiController]
[Route("api/testcomplaints")] // ← tymczasowe
public class TestComplaintsController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping() => Ok("✅ Działa TestComplaintsController");

    [HttpGet("whoami")]
    public IActionResult WhoAmI()
    {
        var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
        var name = User.Identity?.Name ?? "brak";
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "null";
        var roles = User.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        return Ok(new
        {
            isAuthenticated,
            name,
            userId,
            roles
        });
    }

}
