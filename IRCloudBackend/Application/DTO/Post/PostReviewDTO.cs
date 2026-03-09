namespace IRCloudBackend.Application.DTO.Post;

public class PostReviewDTO
{
    public string AuthorUsername { get; set; } = "";
    public int Rating { get; set; }
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
