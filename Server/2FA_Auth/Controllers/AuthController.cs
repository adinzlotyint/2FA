using Microsoft.AspNetCore.Mvc;
using Data;
using Models;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase {
  private readonly AppDbContext _context;

  public AuthController(AppDbContext context) {
    _context = context;
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterRequest request) {
    if (await _context.Users.AnyAsync(u => u.Username == request.Username)) {
      return BadRequest("Username already exists.");
    }

    var user = new User {
      Username = request.Username,
      PasswordHash = HashPassword(request.Password), // implement hashing
      Email = request.Email
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    // Optionally add default 2FA settings
    var user2FA = new User2FASettings {
      UserId = user.Id,
      Method = "None",
      SecretKey = "",
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow
    };
    _context.User2FASettings.Add(user2FA);
    await _context.SaveChangesAsync();

    return Ok(new { Message = "Registration successful." });
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginRequest request) {
    var user = await _context.Users
        .FirstOrDefaultAsync(u => u.Username == request.Username);

    if (user == null || !VerifyPassword(user.PasswordHash, request.Password)) {
      return Unauthorized("Invalid credentials.");
    }

    // Check if 2FA is enabled
    var user2FA = await _context.User2FASettings
        .FirstOrDefaultAsync(s => s.UserId == user.Id);

    if (user2FA != null && user2FA.Method != "None") {
      // Here you can check the 2FA code depending on the method
      // For TOTP: verify TOTP code
      // For SMS/Email: verify code from the request
      // (Implementation details omitted for brevity)

      bool is2FAVerified = Verify2FA(user2FA.Method, user2FA.SecretKey, request.TwoFactorCode);
      if (!is2FAVerified) {
        return Unauthorized("Invalid 2FA code.");
      }
    }

    // If user is valid and 2FA (if any) is correct
    // Return success or a token
    return Ok(new { Message = "Login successful." });
  }

  private string HashPassword(string password) {
    // Implement strong hashing (like BCrypt)
    return password; // placeholder
  }

  private bool VerifyPassword(string storedHash, string providedPassword) {
    // Compare using your hashing method
    return storedHash == providedPassword; // placeholder
  }

  private bool Verify2FA(string method, string secretKey, string code) {
    // Implementation detail for verifying TOTP/SMS/Email
    return true; // placeholder
  }
}

public class RegisterRequest {
  public string Username { get; set; }
  public string Password { get; set; }
  public string Email { get; set; }
}

public class LoginRequest {
  public string Username { get; set; }
  public string Password { get; set; }
  public string TwoFactorCode { get; set; }
}
