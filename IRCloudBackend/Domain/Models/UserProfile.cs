namespace IRCloudBackend.Domain.Models;

public class UserProfile
{
    public int Id { get; set; }
    public Guid ApplicationUserGuid { get; set; }
    public string AvatarUrl { get; set; } = "";
    public string Bio { get; set; } = "";
    public ICollection<Post> CreatedPosts { get; set; } = new List<Post>();
    public ICollection<Review> CreatedReviews { get; set; } = new List<Review>();
    public ICollection<Post> SavedPosts { get; set; } = new List<Post>();
    public ICollection<UserProfile> FollowedUsers { get; set; } = new List<UserProfile>();
    public ICollection<UserProfile> FollowingUsers { get; set; } = new List<UserProfile>();
}
