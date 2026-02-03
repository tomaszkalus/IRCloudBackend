namespace IRCloudBackend.Application.DTO.Category;

public class EditCategoryRequest
{
    public string Name { get; set; } = "";
    public bool IsEnabled { get; set; } = true;
    public int? ParentCategoryId { get; set; }
}
