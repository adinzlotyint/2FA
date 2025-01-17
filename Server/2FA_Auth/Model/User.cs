using System.ComponentModel.DataAnnotations;

namespace Auth2FA.Model {
  public class User {
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [EmailAddress]
    public string Email { get; set; }
  }
}
