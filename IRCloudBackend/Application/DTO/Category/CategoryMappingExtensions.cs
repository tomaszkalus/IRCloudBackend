using System.Linq.Expressions;

namespace IRCloudBackend.Application.DTO.Category;

public static class CategoryMappingExtensions
{
    public static CategoryDto ToDto(this Domain.Models.Category category) =>
        new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            IsEnabled = category.IsEnabled,
            ParentId = category.ParentId,
            SubCategories = category.Children.Select(c => c.ToDto()).ToList()
        };

    public static Domain.Models.Category ToEntity(this UpsertCategoryRequest dto) =>
        new Domain.Models.Category
        {
            Name = dto.Name,
            ParentId = dto.ParentCategoryId,
            IsEnabled = dto.IsEnabled,
        };
}
