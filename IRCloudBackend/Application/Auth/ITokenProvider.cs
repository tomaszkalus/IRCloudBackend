using IRCloudBackend.Infrastructure.Identity;

namespace IRCloudBackend.Application.Auth;

public interface ITokenProvider
{
    public string CreateToken(ApplicationUser user, IList<string> roles);
    public string GenerateRefreshToken();
}
