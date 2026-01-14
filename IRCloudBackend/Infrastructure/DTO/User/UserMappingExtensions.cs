using IRCloudBackend.Infrastructure.Identity;

namespace IRCloudBackend.Infrastructure.DTO.User;

public static class UserMappingExtensions
{
    public static UserDTO ToDto(this Domain.Models.DomainUser domainUser, ApplicationUser applicationUser) =>
        new UserDTO
        {
            Username = applicationUser.UserName ?? "",
            AvatarUrl = domainUser.AvatarUrl ?? "",
            Bio = domainUser.Bio ?? "",
            FollowersCount = domainUser.FollowingUsers.Count()
        };
}
