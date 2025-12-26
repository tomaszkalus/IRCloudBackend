using System.Security.Claims;

using IRCloudBackend.Domain.Models;
using IRCloudBackend.Infrastructure.DbContexts;
using IRCloudBackend.Infrastructure.DTO.Post;
using IRCloudBackend.Infrastructure.DTO.User;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IRCloudBackend.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/user/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserProfile(string username)
        {
            var applicationUser = await _context.Users.FirstOrDefaultAsync(user => user.UserName == username);

            if (applicationUser == null)
            {
                return NotFound();
            }

            var domainUser = await _context.UserProfiles.FirstOrDefaultAsync(user => user.ApplicationUserGuid == applicationUser.Id);

            if (domainUser == null)
            {
                return NotFound();
            }

            var userDTO = domainUser.ToDto(applicationUser);
            return userDTO;
        }

        // PATCH: api/user/
        [HttpPatch]
        public async Task<IActionResult> PatchUserProfile(UpdateUserProfileRequest request)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var currentUser = await _context.UserProfiles.FirstOrDefaultAsync(u => u.ApplicationUserGuid == userId);
            if (currentUser == null)
            {
                return BadRequest();
            }

            if(request.AvatarUrl != null)
            {
                currentUser.UpdateAvatarUrl(request.AvatarUrl);
            }

            if(request.Bio != null)
            {
                currentUser.UpdateBio(request.Bio);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/user/saved-posts/
        [HttpPost]
        public async Task<IActionResult> AddPostToSaved([FromBody] int postId)
        {
            Post? post = await _context.Posts.FirstOrDefaultAsync(post => post.Id == postId);
            if (post == null)
            {
                return NotFound();
            }

            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var currentUser = await _context.UserProfiles.FirstOrDefaultAsync(u => u.ApplicationUserGuid == userId);

            if (currentUser == null)
            {
                return BadRequest();
            }

            currentUser.AddPostToSavedPosts(post);
            return NoContent();
        }

        // GET: api/user/saved-posts/
        [HttpGet]
        public async Task<IActionResult> GetSavedPosts()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var currentUser = await _context.UserProfiles.FirstOrDefaultAsync(u => u.ApplicationUserGuid == userId);

            if (currentUser == null)
            {
                return BadRequest();
            }

            // TODO map the usernames to saved post DTO
            var savedPosts = currentUser.SavedPosts.Select(p => p.ToSavedPostDTO());

            return Ok(savedPosts);
        }
    }
}
