using IRCloudBackend.Domain.Models;
using IRCloudBackend.Infrastructure.DbContexts;
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

        // GET: api/UserProfile/5
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

        // PUT: api/UserProfile/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserProfile(UpdateUserProfileDTO userProfileDTO)
        {
            // TODO
        }

        // POST: api/UserProfile
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserProfile>> PostUserProfile(UserProfile userProfile)
        {
            _context.UserProfile.Add(userProfile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserProfile", new { id = userProfile.Id }, userProfile);
        }

        // DELETE: api/UserProfile/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserProfile(int id)
        {
            var userProfile = await _context.UserProfile.FindAsync(id);
            if (userProfile == null)
            {
                return NotFound();
            }

            _context.UserProfile.Remove(userProfile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserProfileExists(int id)
        {
            return _context.UserProfile.Any(e => e.Id == id);
        }
    }
}
