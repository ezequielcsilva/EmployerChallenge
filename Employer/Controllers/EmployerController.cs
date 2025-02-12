using Employer.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employer.Controllers;

[ApiController]
[Route("api/employer")]
public class EmployerController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EmployerController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("welcome")]
    public async Task<IActionResult> GetUser([FromQuery] string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return BadRequest("Username is required.");
        }

        var user = await _context.Users
            .Where(u => u.Username == username)
            .Select(u => new { u.Name })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Ok(new { Message = $"Hello, {user.Name}" });
    }
}
