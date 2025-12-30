namespace IRCloudBackend.Infrastructure.DTO.Post;

public class PostDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public int NumberOfSaves { get; set; }
    public List<PostCategoryDTO> Categories { get; set; } = new();
    public List<PostReviewDTO> Reviews { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public string AuthorUsername { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
