using IRCloudBackend.Domain.Models;
using IRCloudBackend.Infrastructure.Identity;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IRCloudBackend.Infrastructure.DbContexts;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<DomainUser> DomainUsers { get; set; }
    public DbSet<ConfirmationToken> ConfirmationTokens { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<IrFile> IrFiles { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

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

        // Post Validation
        builder.Entity<Post>()
            .Property(p => p.Title).HasMaxLength(60);

        builder.Entity<Post>()
            .Property(p => p.Description).HasMaxLength(500);


        // Review
        builder.Entity<Review>()
            .ToTable(tb => tb.HasCheckConstraint("CK_Review_Rating_Range", "rating >= 0 AND rating <= 5"));

        builder.Entity<Review>()
            .HasOne(r => r.Post)
            .WithMany(p => p.Reviews)
            .HasForeignKey(p => p.PostId)
            .IsRequired();

        builder.Entity<Review>()
            .HasOne(r => r.Author)
            .WithMany(e => e.CreatedReviews)
            .HasForeignKey(e => e.AuthorId);

        // Review Validation
        builder.Entity<Review>()
            .Property(r => r.Content)
            .HasMaxLength(500);

        // PasswordResetToken
        builder.Entity<PasswordResetToken>()
            .HasKey(e => e.Token);

        // ConfirmationToken
        builder.Entity<ConfirmationToken>()
            .HasKey(e => e.Token);

        // UserProfile
        builder.Entity<DomainUser>()
            .HasMany(u => u.SavedPosts)
            .WithMany(p => p.SavingUsers);

        builder.Entity<DomainUser>()
            .HasMany(u => u.FollowedUsers)
            .WithMany(u => u.FollowingUsers)
            .UsingEntity<Dictionary<string, object>>(
                "UserFollows",
                u => u.HasOne<DomainUser>().WithMany().HasForeignKey("FollowedId"),
                u => u.HasOne<DomainUser>().WithMany().HasForeignKey("FollowerId")
            );

        // UserProfile Validation
        builder.Entity<DomainUser>()
            .Property(u => u.Bio)
            .HasMaxLength(500);

        // Category
        builder.Entity<Category>()
            .HasMany(c => c.Children)
            .WithOne()
            .HasForeignKey(c => c.ParentId);
    }
}
