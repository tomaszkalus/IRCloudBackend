using IRCloudBackend.Infrastructure.Identity;

namespace IRCloudBackend.Domain.Models;
public class Review
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public required Post Post { get; set; }  
    public int AuthorId { get; set; }
    public required UserProfile Author { get; set; }
    public required int Rating { get; set; }
    public string? Content { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}
