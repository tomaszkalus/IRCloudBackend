namespace IRCloudBackend.Infrastructure.DTO.Category;

public class UpsertCategoryRequest
{
    public string Name { get; set; } = "";
    public bool IsEnabled { get; set; } = true;
    public int? ParentCategoryId { get; set; }
}
