using System.Linq.Expressions;

using IRCloudBackend.Infrastructure.DTO.Category;

namespace IRCloudBackend.Domain.Services;

public class CategoryService
{
    public static Expression<Func<Models.Category, CategoryDto>> GetCategoryProjection(int maxDepth, int currentDepth = 0)
    {
        currentDepth++;

        Expression<Func<Models.Category, CategoryDto>> result = category => new CategoryDto()
        {
            Id = category.Id,
            Name = category.Name,
            SubCategories = currentDepth == maxDepth
                ? new List<CategoryDto>()
                : category.Children.AsQueryable()
                    .Select(GetCategoryProjection(maxDepth, currentDepth))
                    .ToList()
        };

        return result;
    }
}
