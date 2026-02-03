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

    /// <summary>
    /// Retrieves all categories as a list of tree structures, recursively.
    /// </summary>
    /// <returns>DTO of all the top level categories and their descendants</returns>
    public async Task<IReadOnlyList<CategoryDto>> GetAllCategoriesAsync() => await _context
                .Categories
                .Where(c => c.ParentId == null)
                .Select(GetCategoryProjection(7, 0))
                .ToListAsync();

    /// <summary>
    /// Retrieves a category with a given ID, as a tree structure, recursively.
    /// </summary>
    /// <param name="categoryId">ID of the category to retrieve</param>
    /// <returns></returns>
    public async Task<CategoryDto> GetCategory(int categoryId) => await _context
                .Categories
                .Where(c => c.Id == categoryId)
                .Select(GetCategoryProjection(7, 0))
                .FirstAsync();

    /// <summary>
    /// Retrieves a category with a given ID, and all of its ancestors, in a list, from the root to the leaf.
    /// </summary>
    /// <param name="categoryId">ID of the category to retrieve</param>
    /// <param name="ct"></param>
    /// <returns>List of all the ancestors of the category</returns>
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

        result.Reverse();
        return result;
    }

    private Expression<Func<Domain.Models.Category, CategoryDto>> GetCategoryProjection(int maxDepth, int currentDepth = 0)
    {
        currentDepth++;

        Expression<Func<Domain.Models.Category, CategoryDto>> result = category => new CategoryDto()
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
