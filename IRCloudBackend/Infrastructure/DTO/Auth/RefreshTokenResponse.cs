namespace IRCloudBackend.Infrastructure.DTO.Auth;

public class RefreshTokenResponse
{
    public string Token { get; set; }
    public DateTime ExpiresOnUtc { get; set; }
}
