using System.Runtime.InteropServices;

namespace IRCloudBackend.Domain.Models;
public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public required int AuthorId { get; set; }
    public DomainUser Author { get; set; } = null;
    public DateTime CreatedAt { get; set; }
    public required int CategoryId { get; set; }
    public Category Category { get; set; } = null;
    public ICollection<IrFile> IrFiles { get; set; } = new List<IrFile>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<DomainUser> SavingUsers { get; set; } = new List<DomainUser>();
}
