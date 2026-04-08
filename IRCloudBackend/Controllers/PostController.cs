using System.Security.Claims;

using IRCloudBackend.Application.DTO.Post;
using IRCloudBackend.Application.Services;
using IRCloudBackend.Domain.Models;
using IRCloudBackend.Infrastructure.DbContexts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IRCloudBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;

        public PostController(PostService postService, ApplicationDbContext applicationDbContext, IAuthorizationService authorizationService)
        {
            _postService = postService;
            _context = applicationDbContext;
            _authorizationService = authorizationService;
        }

        // GET: api/Post/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO>> GetPost(int id, CancellationToken ct)
        {
            var postDto = await _postService.GetPostAsync(id, ct);
            return postDto == null ? NotFound() : Ok(postDto);
        }

        // PUT: api/Post/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> EditPost(int id, EditPostRequest request)
        {
            var post = await _context.Posts.Include(p => p.Author).FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, post, "PostOwnershipPolicy");

            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            await _postService.EditPostAsync(request, post);
            return NoContent();
        }

        // POST: api/Post
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Post>> CreatePost(CreatePostRequest request)
        {
            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                return Unauthorized();
            }

            bool success = await _postService.CreatePostAsync(request, userId);
            return success ? NoContent() : NotFound("Category with a given ID does not exist");
        }

        // DELETE: api/Post/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.Include(p => p.Author)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            var authorizationResult = await _authorizationService.AuthorizeAsync(User, post, "PostOwnershipPolicy");
            if (!authorizationResult.Succeeded)
            {
                return Forbid();
            }

            if (!Guid.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                return Unauthorized();
            }

            await _postService.DeletePostAsync(post, userId);
            return NoContent();
        }
    }
}
