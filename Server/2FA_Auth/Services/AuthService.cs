using Auth2FA.Model;
using Data;
using Microsoft.EntityFrameworkCore;
using Requests;

namespace Auth2FA.Services {
  public class AuthService {
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context) {
      _context = context;
    }

    public async Task<bool> IsUsernameTakenAsync(string username) {
      return await _context.Users.AnyAsync(u => u.Username == username);
    }

    public async Task RegisterUserAsync(RegisterRequest request) {
      var user = new User {
        Username = request.Username,
        PasswordHash = HashPassword(request.Password),
        Email = request.Email
      };

      _context.Users.Add(user);
      await _context.SaveChangesAsync();

      var user2FA = new User2FASettings {
        UserId = user.Id,
        Method = "None",
        SecretKey = "",
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
      };

      _context.User2FASettings.Add(user2FA);
      await _context.SaveChangesAsync();
    }

    public async Task<User?> AuthenticateUserAsync(string username, string password) {
      var user = await _context.Users
          .FirstOrDefaultAsync(u => u.Username == username);

      if (user == null || !VerifyPassword(user.PasswordHash, password)) {
        return null;
      }

      return user;
    }

    public async Task<User2FASettings?> GetUser2FASettingsAsync(int userId) {
      return await _context.User2FASettings.FirstOrDefaultAsync(s => s.UserId == userId);
    }

    // Private helper methods
    private string HashPassword(string password) {
      return password; // Implement real hashing
    }

    private bool VerifyPassword(string storedHash, string providedPassword) {
      return storedHash == providedPassword; // Implement real verification
    }
  }

}
