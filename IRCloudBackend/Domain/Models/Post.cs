namespace IRCloudBackend.Domain.Models;
public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public int AuthorId { get; set; }
    public required UserProfile Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CategoryId { get; set; }
    public required Category Category { get; set; }
    public ICollection<IrFile> IrFiles { get; set; } = new List<IrFile>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
