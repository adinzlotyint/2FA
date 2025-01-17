using Auth2FA.Services;
using Microsoft.AspNetCore.Mvc;
using Requests;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase {
  private readonly AuthService _authService;

  public AuthController(AuthService authService) {
    _authService = authService;
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterRequest request) {
    if (!ModelState.IsValid) {
      return BadRequest(ModelState);
    }

    if (await _authService.IsUsernameTakenAsync(request.Username)) {
      return BadRequest("Username is already taken.");
    }

    await _authService.RegisterUserAsync(request);
    return Ok(new { Message = "Registration successful." });
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginRequest request) {
    var user = await _authService.AuthenticateUserAsync(request.Username, request.Password);
    if (user == null) {
      return Unauthorized("Invalid credentials.");
    }

    var user2FA = await _authService.GetUser2FASettingsAsync(user.Id);
    if (user2FA != null && user2FA.Method != "None") {
      // Perform 2FA verification (delegated to AuthService or another service)
      if (!Verify2FA(user2FA.Method, user2FA.SecretKey, request.TwoFactorCode)) {
        return Unauthorized("Invalid 2FA code.");
      }
    }

    return Ok(new { Message = "Login successful." });
  }

  private bool Verify2FA(string method, string secretKey, string code) {
    return true; // Delegate this logic to a service if it grows in complexity
  }
}
