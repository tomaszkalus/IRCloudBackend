namespace IRCloudBackend.Application.DTO.Category;

public class AddCategoryRequest
{
    public string Name { get; set; } = "";
    public bool IsEnabled { get; set; } = true;
    public int? ParentCategoryId { get; set; }
}
