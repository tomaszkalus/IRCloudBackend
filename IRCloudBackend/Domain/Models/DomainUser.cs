using System;

namespace IRCloudBackend.Domain.Models;

public class DomainUser
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public Guid ApplicationUserGuid { get; set; }
    public string? AvatarUrl { get; set; } = "";
    public string? Bio { get; set; } = "";
    public ICollection<Post> CreatedPosts { get; set; } = new List<Post>();
    public ICollection<Review> CreatedReviews { get; set; } = new List<Review>();
    public ICollection<Post> SavedPosts { get; set; } = new List<Post>();
    public ICollection<DomainUser> FollowedUsers { get; set; } = new List<DomainUser>();
    public ICollection<DomainUser> FollowingUsers { get; set; } = new List<DomainUser>();

    public void UpdateBio(string bio)
    {
        if (string.IsNullOrWhiteSpace(bio))
            throw new ArgumentException("Bio cannot be empty.");

        if (bio.Length < 8)
            throw new ArgumentException("Bio cannot be shorter than 8 characters.");

        if (bio.Length > 500)
            throw new ArgumentException("Bio cannot be longer than 500 characters.");

        Bio = bio;
    }

    public void UpdateAvatarUrl(string avatarUrl)
    {
        if (string.IsNullOrWhiteSpace(avatarUrl))
            throw new ArgumentException("Avatar URL cannot be empty.");

        if (avatarUrl.Length < 5)
            throw new ArgumentException("Avatar URL cannot be shorter than 5 characters.");

        if (avatarUrl.Length > 2048)
            throw new ArgumentException("Avatar URL cannot be longer than 2048 characters.");

        if (!Uri.IsWellFormedUriString(avatarUrl, UriKind.Absolute))
            throw new ArgumentException("Avatar URL is invalid.");

        AvatarUrl = avatarUrl;
    }

    public void AddPostToSavedPosts(Post post)
    {
        if (post == null)
            throw new ArgumentNullException("Post cannot be null");

        if (!SavedPosts.Contains(post))
        {
            SavedPosts.Add(post);
        }
    }

    public void RemovePostFromSavedPosts(Post post)
    {
        if (post == null)
            throw new ArgumentNullException("Post cannot be null");

        if (SavedPosts.Contains(post))
        {
            SavedPosts.Remove(post);
        }
    }
}
