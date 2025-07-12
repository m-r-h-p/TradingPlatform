using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TradingPlatform.Core.Entities;

namespace TradingPlatform.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> users { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // Configure relationships
            builder.Entity<Trade>()
                 .HasOne(t => t.User)
                 .WithMany()
                 .HasForeignKey(t => t.UserId);

            // Configure RefreshToken
            builder.Entity<RefreshToken>()
                .HasKey(rt => rt.Id);
            builder.Entity<RefreshToken>()
                .Property(rt => rt.UserId)
                .IsRequired();
            builder.Entity<RefreshToken>()
                .Property(rt => rt.Token)
                .IsRequired();
        }
    }
}
