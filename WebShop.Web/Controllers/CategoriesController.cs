using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models.Entities;
using WebShop.Web.ViewModels;

namespace WebShop.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly WebShopContext ctx;
        private readonly IMapper mapper;

        public CategoriesController(WebShopContext ctx, IMapper mapper)
        {
            this.ctx = ctx;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var categories = mapper.Map<List<CategoryModel>>(ctx.Categories.OrderBy(c => c.Title).ToList());
            return Ok(categories);
        }

        [HttpGet("{id}", Name = "CategoryGet")]
        public IActionResult Get(int id)
        {
            var category = mapper.Map<CategoryModel>(ctx.Categories
                                                        .Where(c => c.Id == id)
                                                        .Include(c => c.Products)
                                                        .FirstOrDefault());
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CategoryInputModel model)
        {
            var entity = mapper.Map<Category>(model);
            this.ctx.Add(entity);
            if (await this.ctx.SaveChangesAsync() > 0)
            {
                var url = Url.Link("CategoryGet", new { id = entity.Id });
                return Created(url, mapper.Map<CategoryModel>(entity));
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]CategoryInputModel model)
        {
            var oldCategory = this.ctx.Categories.FirstOrDefault(c => c.Id == id);
            if (oldCategory == null)
            {
                return NotFound();
            }

            oldCategory.Title = model.Title;
            oldCategory.ParentId = model.ParentId;
            if (await this.ctx.SaveChangesAsync() > 0)
            {
                return Ok(oldCategory);
            }

            return BadRequest();
        }

        // TODO: check delete when products exist and check delete when children exist
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = this.ctx.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            this.ctx.Categories.Remove(category);
            if (await this.ctx.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}