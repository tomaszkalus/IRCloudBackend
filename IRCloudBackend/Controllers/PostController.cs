using System.Security.Claims;

using IRCloudBackend.Application.DTO.Post;
using IRCloudBackend.Application.Services;
using IRCloudBackend.Domain.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IRCloudBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;

        public PostController(PostService postService)
        {
            _postService = postService;
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
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            bool success = await _postService.EditPostAsync(id, request, new Guid(userId));
            return success ? NoContent() : NotFound();
        }

        // POST: api/Post
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Post>> CreatePost(CreatePostRequest request)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            bool success = await _postService.CreatePostAsync(request, new Guid(userId));

            return success ? NoContent() : NotFound();
        }

        // DELETE: api/Post/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            bool success = await _postService.DeletePostAsync(id, new Guid(userId));

            return success ? NoContent() : NotFound();
        }
    }
}
