using Microsoft.AspNetCore.Mvc;
using Data;
using Auth2FA.Model.Requests;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase {
  private readonly AppDbContext _context;

  public UsersController(AppDbContext context) {
    _context = context;
  }

  [HttpGet("{userId}/2fa")]
  public async Task<IActionResult> Get2FASettings(int userId) {
    var settings = await _context.User2FASettings
        .FirstOrDefaultAsync(s => s.UserId == userId);

    if (settings == null)
      return NotFound("2FA Settings not found.");

    return Ok(settings);
  }

  [HttpPut("{userId}/2fa")]
  public async Task<IActionResult> Update2FASettings(int userId, [FromBody] Update2FARequest request) {
    var settings = await _context.User2FASettings
        .FirstOrDefaultAsync(s => s.UserId == userId);

    if (settings == null)
      return NotFound("2FA Settings not found.");

    settings.Method = request.Method;
    settings.SecretKey = request.SecretKey;
    settings.UpdatedAt = DateTime.UtcNow;

    await _context.SaveChangesAsync();

    return Ok(new { Message = "2FA settings updated." });
  }
}


