using IRCloudBackend.Application.DTO.Post;
using IRCloudBackend.Domain.Models;
using IRCloudBackend.Infrastructure.DbContexts;

using Microsoft.EntityFrameworkCore;

namespace IRCloudBackend.Application.Services;

public class PostService
{
    private readonly ApplicationDbContext _context;
    private readonly CategoryService _categoryService;

    public PostService(ApplicationDbContext context, CategoryService categoryService)
    {
        _context = context;
        _categoryService = categoryService;
    }

    public async Task<PostDTO?> GetPostAsync(int postId, CancellationToken ct)
    {

        var post = await _context.Posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(p => p.Id == postId);

        if (post == null)
        {
            return null;
        }

        var categories = await _categoryService.GetCategoryPathAsync(post.CategoryId, ct);

        var postDto = new PostDTO
        {
            Title = post.Title,
            Description = post.Description,
            CreatedAt = post.CreatedAt,
            AuthorUsername = post.Author.Username,
            CategoryPath = categories.ToList(),
        };

        return postDto;
    }

    public async Task<bool> EditPostAsync(int id, EditPostRequest request, Guid applicationUserId)
    {
        var domainUser = await _context.DomainUsers.FirstOrDefaultAsync(u => u.ApplicationUserGuid == applicationUserId);

        if (domainUser == null)
        {
            throw new ApplicationException();
        }

        var post = await _context.Posts.FindAsync(id);

        if (post == null)
        {
            return false;
        }

        if (domainUser.Id != post.AuthorId)
        {
            return false;
        }

        post.Title = request.Title;
        post.Description = request.Description;
        post.CategoryId = request.CategoryId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CreatePostAsync(CreatePostRequest request, Guid applicationUserId)
    {
        var domainUser = await _context.DomainUsers.FirstOrDefaultAsync(u => u.ApplicationUserGuid == applicationUserId);

        if (domainUser == null)
        {
            return false;
        }

        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId);
        if (!categoryExists)
        {
            return false;
        }

        var post = new Post()
        {
            AuthorId = domainUser.Id,
            CategoryId = request.CategoryId,
            CreatedAt = DateTime.UtcNow,
            Title = request.Title,
            Description = request.Description,
        };

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePostAsync(int id, Guid applicationUserId)
    {
        var domainUser = await _context.DomainUsers.FirstOrDefaultAsync(u => u.ApplicationUserGuid == applicationUserId);

        if (domainUser == null)
        {
            return false;
        }

        var post = await _context.Posts.FindAsync(id);
        if (post == null)
        {
            return false;
        }

        if(post.AuthorId != domainUser.Id)
        {
            return false;
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();

        return true;
    }
}
