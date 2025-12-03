namespace IRCloudBackend.Infrastructure.DTO.User;

public class UserDTO
{
    public string Username { get; set; } = "";
    public string AvatarUrl { get; set; } = "";
    public string Bio { get; set; } = "";
    public int FollowersCount { get; set; }
    public DateOnly JoinedDate { get; set; }
}
