using Auth2FA.Model.Requests;
using Auth2FA.Model;
using Auth2FA.Services;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase {
  private readonly AppDbContext _context;
  private readonly TotpService _totpService;

  public UsersController(AppDbContext context, TotpService totpService) {
    _context = context;
    _totpService = totpService;
  }

  // GET api/users/{userId}/2fa
  [HttpGet("{userId}/2fa")]
  public async Task<IActionResult> Get2FASettings(int userId) {
    var settings = await _context.User2FASettings
        .FirstOrDefaultAsync(s => s.UserId == userId);

    if (settings == null)
      return NotFound(new { message = "No 2FA settings found for that user." });

    return Ok(new {
      user2FASettingsId = settings.User2FASettingsId,
      userId = settings.UserId,
      method = settings.Method,
      secretKey = settings.SecretKey, // Ensure this is included
      createdAt = settings.CreatedAt,
      updatedAt = settings.UpdatedAt
    });
  }

  // PUT api/users/{userId}/2fa
  [HttpPut("{userId}/2fa")]
  public async Task<IActionResult> Update2FASettings(int userId, [FromBody] Update2FARequest request) {
    // Verify that the user exists
    var user = await _context.Users.FindAsync(userId);
    if (user == null) {
      return NotFound(new { message = "User not found." });
    }

    // Attempt to find existing 2FA settings
    var userSettings = await _context.User2FASettings
        .FirstOrDefaultAsync(s => s.UserId == userId);

    if (userSettings == null) {
      // No existing settings; create new
      userSettings = new User2FASettings {
        UserId = userId,
        Method = request.Method,
        SecretKey = request.SecretKey ?? "",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
      };

      // If method is TOTP and SecretKey not provided, generate it
      if (request.Method == "TOTP" && string.IsNullOrEmpty(request.SecretKey)) {
        userSettings.SecretKey = _totpService.GenerateTOTPSecret();
      }

      _context.User2FASettings.Add(userSettings);
    } else {
      // Update existing settings
      if (request.Method == "TOTP" && string.IsNullOrEmpty(request.SecretKey)) {
        request.SecretKey = _totpService.GenerateTOTPSecret();
      }

      userSettings.Method = request.Method;
      userSettings.SecretKey = request.SecretKey ?? "";
      userSettings.UpdatedAt = DateTime.UtcNow;

      _context.User2FASettings.Update(userSettings);
    }

    try {
      await _context.SaveChangesAsync();
    } catch (DbUpdateException ex) {
      return StatusCode(500, new { message = "An error occurred while updating 2FA settings.", detail = ex.InnerException?.Message });
    }

    return Ok(new {
      user2FASettingsId = userSettings.User2FASettingsId,
      userId = userSettings.UserId,
      method = userSettings.Method,
      secretKey = userSettings.SecretKey,
      createdAt = userSettings.CreatedAt,
      updatedAt = userSettings.UpdatedAt
    });
  }
}
