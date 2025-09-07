using Leaderone.Application.DTOs;
using Leaderone.Application.Interfaces;
using Leaderone.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Leaderone.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController(ICategoryRepository categoryRepository) : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        [HttpGet("tenants/{tenantId:guid}")]
        public async Task<ActionResult<List<CategoryDTO>>> GetAllCategoriesAsync(Guid tenantId)
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync(tenantId);

            var result = categories?.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            });

            return Ok(result);

        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetCategoryAsync(id);

            var result = category is null ? null : new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(null!, "Category name is required");
                return BadRequest("Category name is required");
            }

            if (await _categoryRepository.IsCategoryNameUniqueAsync(categoryDTO.Name))
            {
                ModelState.AddModelError(null!, "Category name must be unique");
                return BadRequest("Category name must be unique");
            }

            var category = new Category
            {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description,
                TenantId = categoryDTO.TenantId
            };
            await _categoryRepository.AddCategoryAsync(category);
            return Ok("Your category has been added");
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCategoryAsync(Guid id, [FromBody] CategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(null!, "Category name is required");
                return BadRequest("Category name is required");
            }

            var existingCategory = await _categoryRepository.GetCategoryAsync(id);
            if (existingCategory is null)
            {
                return NotFound("Category not found");
            }

            if (await _categoryRepository.IsCategoryNameUniqueAsync(id, categoryDTO.Name))
            {
                ModelState.AddModelError(null!, "Category name must be unique");
                return BadRequest("Category name must be unique");
            }

            existingCategory.Name = categoryDTO.Name;
            existingCategory.Description = categoryDTO.Description;
            await _categoryRepository.UpdateCategoryAsync(existingCategory);
            return Ok("Your category has been updated");
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCategoryAsync(Guid id)
        {
            var existingCategory = await _categoryRepository.GetCategoryAsync(id);
            if (existingCategory is null)
            {
                return NotFound("Category not found");
            }

            await _categoryRepository.DeleteCategoryAsync(id);
            return Ok("Your category has been deleted");
        }




    }
}
