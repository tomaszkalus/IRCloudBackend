namespace IRCloudBackend.Infrastructure.Identity;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpiresOnUtc { get; set; }
    public ApplicationUser User { get; set; }
}
