namespace IRCloudBackend.Infrastructure.DTO.Post;

public class SavedPostDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public int AuthorId { get; set; }
    public string AuthorName { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
