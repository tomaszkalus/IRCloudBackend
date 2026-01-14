using IRCloudBackend.Domain.Models;
using IRCloudBackend.Infrastructure.DbContexts;
using IRCloudBackend.Infrastructure.DTO.Post;
using IRCloudBackend.Infrastructure.Identity;

namespace IRCloudBackend.Domain.Services;

public class PostService
{
    private readonly ApplicationDbContext _context;
    private readonly CategoryService _categoryService;

    public PostService(ApplicationDbContext context, CategoryService categoryService)
    {
        _context = context;
        _categoryService = categoryService;
    }

    public async Task<PostDTO> GetPostAsync(Post post, ApplicationUser applicationUser, CancellationToken cancellationToken)
    {

        var categories = await _categoryService.GetCategoryPathAsync(post.CategoryId, new CancellationToken());

        return new PostDTO
        {
            CreatedAt = post.CreatedAt,
            AuthorUsername = applicationUser.UserName,
            Categories = categories.ToList(),
            Description = post.Description,
            Id = post.Id,
            NumberOfSaves = post.SavingUsers.Count,

        };
    }
}
