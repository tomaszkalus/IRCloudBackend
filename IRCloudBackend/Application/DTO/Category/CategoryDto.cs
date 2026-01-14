namespace IRCloudBackend.Application.DTO.Category;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int? ParentId { get; set; }
    public bool IsEnabled { get; set; }
    public List<CategoryDto> SubCategories { get; set; } = new List<CategoryDto>();
}
