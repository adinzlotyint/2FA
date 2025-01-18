using Auth2FA.Model;
using Auth2FA.Model.Requests;
using Auth2FA.Services;
using Data;
using Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auth2FA.Controllers {
  [ApiController]
  [Route("api/[controller]")]
  public class AuthController : ControllerBase {
    private readonly AuthService _authService;
    private readonly TotpService _totpService;

    public AuthController(AuthService authService, TotpService totpService) {
      _authService = authService;
      _totpService = totpService;
    }

    // POST api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request) {
      if (await _authService.IsUsernameTakenAsync(request.Username)) {
        return BadRequest(new { message = "Username is already taken." });
      }

      await _authService.RegisterUserAsync(request);
      return Ok(new { message = "Registration successful" });
    }

    // POST api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request) {
      var user = await _authService.AuthenticateUserAsync(request.Username, request.Password);
      if (user == null) {
        return Unauthorized(new { message = "Invalid username or password." });
      }

      var user2FA = await _authService.GetUser2FASettingsAsync(user.Id);
      if (user2FA == null || user2FA.Method == "None") {
        return Ok(new {
          message = "Login successful.",
          username = user.Username,
          userId = user.Id
        });
      }

      if (user2FA.Method == "TOTP") {
        if (string.IsNullOrEmpty(request.TwoFactorCode)) {
          return BadRequest(new { message = "TwoFactor code is required for this account.", requires2FA = true, userId = user.Id });
        }

        var is2FAValid = _authService.Verify2FA(user2FA.Method, user2FA.SecretKey, request.TwoFactorCode);
        if (!is2FAValid) {
          return Unauthorized(new { message = "Invalid 2FA code." });
        }

        return Ok(new {
          message = "Login successful (2FA verified).",
          username = user.Username,
          userId = user.Id
        });
      }

      return Unauthorized(new { message = "Invalid 2FA method." });
    }
  }
}
