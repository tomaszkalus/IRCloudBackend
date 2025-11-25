using System.Linq.Expressions;

using IRCloudBackend.Domain.Models;
using IRCloudBackend.Domain.Services;
using IRCloudBackend.Infrastructure.DbContexts;
using IRCloudBackend.Infrastructure.DTO.Category;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IRCloudBackend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all the top-level categories.
        /// </summary>
        /// <returns>List of all the top-level categories, with the included subcategories.</returns>
        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            return await _context
                .Categories
                .Where(c => c.ParentId == null)
                .Select(CategoryService.GetCategoryProjection(7, 0))
                .ToListAsync();
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Category/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, UpsertCategoryRequest dto)
        {
            Category? category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            Category updatedCategory = dto.ToEntity();
            updatedCategory.Id = id;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Category
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory(UpsertCategoryRequest dto)
        {
            Category category = dto.ToEntity();
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
