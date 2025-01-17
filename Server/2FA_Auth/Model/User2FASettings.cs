using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth2FA.Model {
  public class User2FASettings {
    [Key]
    public int User2FASettingsId { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    public string Method { get; set; }

    public string SecretKey { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public User User { get; set; }
  }
}
