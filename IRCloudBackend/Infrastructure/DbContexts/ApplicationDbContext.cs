using IRCloudBackend.Domain.Models;
using IRCloudBackend.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IRCloudBackend.Infrastructure.DbContexts;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<ConfirmationToken> ConfirmationTokens { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<IrFile> IrFiles { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostTag> PostTags { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Post
        builder.Entity<Post>()
            .HasOne(p => p.Author)
            .WithMany(u => u.CreatedPosts)
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Post>()
            .HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Post>()
            .HasMany(p => p.IrFiles)
            .WithOne(f => f.Post)
            .HasForeignKey(f => f.PostId)
            .IsRequired();

        // Review
        builder.Entity<Review>()
            .HasOne(r => r.Post)
            .WithMany(p => p.Reviews)
            .HasForeignKey(p => p.PostId)
            .IsRequired();

        builder.Entity<Review>()
            .HasOne(r => r.Author)
            .WithMany(e => e.CreatedReviews)
            .HasForeignKey(e => e.AuthorId);

        // PasswordResetToken
        builder.Entity<PasswordResetToken>()
            .HasKey(e => e.Token);

        // ConfirmationToken
        builder.Entity<ConfirmationToken>()
            .HasKey(e => e.Token);

        // User
        builder.Entity<UserProfile>()
            .HasMany(u => u.SavedPosts)
            .WithMany(p => p.SavingUsers);

        builder.Entity<UserProfile>()
            .HasMany(u => u.FollowedUsers)
            .WithMany(u => u.FollowingUsers);
    }
}
