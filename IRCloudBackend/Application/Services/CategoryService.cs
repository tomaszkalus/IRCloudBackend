using System.Linq.Expressions;

using IRCloudBackend.Application.DTO.Category;
using IRCloudBackend.Application.DTO.Post;
using IRCloudBackend.Infrastructure.DbContexts;

using Microsoft.EntityFrameworkCore;

namespace IRCloudBackend.Application.Services;

public class CategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

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

    public async Task<IReadOnlyList<PostCategoryDTO>> GetCategoryPathAsync(
    int categoryId,
    CancellationToken ct)
    {
        var result = new List<PostCategoryDTO>();

        int? currentId = categoryId;

        while (currentId != null)
        {
            var category = await _context.Categories
                .Where(c => c.Id == currentId)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.ParentId
                })
                .SingleAsync(ct);

            result.Add(new PostCategoryDTO
            {
                Id = category.Id,
                Name = category.Name
            });

            currentId = category.ParentId;
        }

        // Reversing the order so the categories are ordered from the root to leaf
        result.Reverse();
        return result;
    }

}
