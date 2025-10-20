namespace IRCloudBackend.Infrastructure.Identity;
public class PasswordResetToken
{
    public string Token = "";
    public int UserId;
    public bool IsActive = true;
    public DateTime CreatedAt;
    public DateTime ExpiresAt;
}
