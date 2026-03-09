using System.Linq.Expressions;

using IRCloudBackend.Application.DTO.Category;
using IRCloudBackend.Application.DTO.Post;
using IRCloudBackend.Domain.Models;
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
    /// Adds a new category.
    /// </summary>
    /// <param name="request">Request for adding the new category</param>
    /// <exception cref="ArgumentException">If the category with the ID of the provided ParentCategoryId does not exist.</exception>
    public async Task AddCategoryAsync(AddCategoryRequest request)
    {
        if (request.ParentCategoryId.HasValue)
        {
            bool parentCategoryExists = await _context.Categories.AnyAsync(c => c.Id == request.ParentCategoryId.Value);
            if (!parentCategoryExists)
            {
                throw new ArgumentException($"Parent category with ID {request.ParentCategoryId} does not exist");
            }
        }

        Category category = request.ToEntity();
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return;
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
