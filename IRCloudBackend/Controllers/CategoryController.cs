using IRCloudBackend.Application.DTO.Category;
using IRCloudBackend.Application.Services;
using IRCloudBackend.Domain.Models;
using IRCloudBackend.Infrastructure.DbContexts;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace IRCloudBackend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly CategoryService _categoryService;

        public CategoryController(ApplicationDbContext context, CategoryService categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        /// <summary>
        /// Gets all the top-level categories.
        /// </summary>
        /// <returns>List of all the top-level categories, with the included subcategories.</returns>
        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Gets the category with a provided ID, with all of its children, recursively
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>The category with all of its descendants</returns>
        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var doesCategoryExist = await _context.Categories.Include(category => category.Children)
                .AnyAsync(c => c.Id == id);

            if (!doesCategoryExist)
            {
                return NotFound();
            }

            var cat = await _categoryService.GetCategory(id);

            return cat;
        }

        /// <summary>
        /// Performs a partial update of a category with the given ID. This endpoint uses RFC 6902 JSON Patch to partially update a category.
        /// </summary>
        /// <param name="id">ID of the category to modify</param>
        /// <param name="patchDocument">Path document</param>
        /// <returns></returns>
        // PATCH: api/Category/5
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] JsonPatchDocument<EditCategoryRequest> patchDocument)
        {
            if (patchDocument is null)
            {
                return BadRequest();
            }

            var category = await _context.Categories.FindAsync(id);

            if (category is null)
            {
                return NotFound();
            }

            var categoryDto = category.ToRequestDto();
            patchDocument.ApplyTo(categoryDto);

            category.Name = categoryDto.Name;
            category.IsEnabled = categoryDto.IsEnabled;
            category.ParentId = categoryDto.ParentCategoryId;

            await _context.SaveChangesAsync();
            return Ok();
        }

        // POST: api/Category
        [HttpPost]
        public async Task<IActionResult> AddCategory(AddCategoryRequest request)
        {
            Category category = request.ToEntity();
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
