using Microsoft.EntityFrameworkCore;
using Models;

namespace Data {
  public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<User2FASettings> User2FASettings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);
    }
  }
}
