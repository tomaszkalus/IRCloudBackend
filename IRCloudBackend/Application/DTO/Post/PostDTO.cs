using IRCloudBackend.Application.DTO.Category;

namespace IRCloudBackend.Application.DTO.Post;

public class PostDTO
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public int NumberOfSaves { get; set; }
    public List<PostCategoryDTO> CategoryPath { get; set; } = new();
    public List<PostReviewDTO> Reviews { get; set; } = new();
    public string AuthorUsername { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
