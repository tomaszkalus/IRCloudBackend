using IRCloudBackend.Infrastructure.Identity;

namespace IRCloudBackend.Infrastructure.DTO.Auth;

public static class RefreshTokenMappingExtensions
{
    public static RefreshTokenResponse ToDto(this RefreshToken token) => new RefreshTokenResponse()
    {
        Token = token.Token,
        ExpiresOnUtc = token.ExpiresOnUtc,
    };
}
