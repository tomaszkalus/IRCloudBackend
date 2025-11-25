namespace IRCloudBackend.Domain.Models;
public class Category
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string Name { get; set; } = "";
    public bool IsEnabled { get; set; }
    public ICollection<Category> Children { get; set; } = new List<Category>();
}
