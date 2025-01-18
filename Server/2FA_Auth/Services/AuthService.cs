using Auth2FA.Model;
using Data;
using Microsoft.EntityFrameworkCore;
using Requests;
using System.Security.Cryptography;

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

      if (user == null || !VerifyPassword(password, user.PasswordHash)) {
        return null;
      }

      return user;
    }

    public async Task<User2FASettings?> GetUser2FASettingsAsync(int userId) {
      return await _context.User2FASettings.FirstOrDefaultAsync(s => s.UserId == userId);
    }

    private string HashPassword(string password) {
      byte[] salt = new byte[16];
      using (var rng = RandomNumberGenerator.Create()) {
        rng.GetBytes(salt);
      }

      // Hash the password with the salt using PBKDF2
      using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256)) {
        byte[] hash = pbkdf2.GetBytes(32); // Get a 256-bit hash
                                           // Combine the salt and hash for storage
        byte[] hashBytes = new byte[48];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 32);

        // Convert to Base64 for storage
        return Convert.ToBase64String(hashBytes);
      }
    }
    private bool VerifyPassword(string providedPassword, string storedHash) {
      // Extract the salt and hash from the stored password
      byte[] hashBytes = Convert.FromBase64String(storedHash);
      byte[] salt = new byte[16];
      Array.Copy(hashBytes, 0, salt, 0, 16);

      // Hash the input password with the same salt
      using (var pbkdf2 = new Rfc2898DeriveBytes(providedPassword, salt, 100000, HashAlgorithmName.SHA256)) {
        byte[] hash = pbkdf2.GetBytes(32);

        // Compare the resulting hash with the stored hash
        for (int i = 0; i < 32; i++) {
          if (hashBytes[i + 16] != hash[i]) {
            return false;
          }
        }
      }

      return true;
    }
    public bool Verify2FA(string method, string secretKey, string code) {

      return true;
    }
  }

}
