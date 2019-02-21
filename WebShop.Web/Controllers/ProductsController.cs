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
    [Authorize]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly WebShopContext ctx;
        private readonly IMapper mapper;

        public ProductsController(WebShopContext ctx, IMapper mapper)
        {
            this.ctx = ctx;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = mapper.Map<List<ProductModel>>(ctx.Products.ToList());
            return Ok(products);
        }

        [HttpGet("{id}", Name = "ProductGet")]
        public IActionResult Get(int id)
        {
            var product = mapper.Map<ProductModel>(ctx.Products.FirstOrDefault(m => m.Id == id));
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ProductInputModel model)
        {
            var newProduct = mapper.Map<Product>(model);
            this.ctx.Add(newProduct);
            if (await this.ctx.SaveChangesAsync() > 0)
            {
                var url = Url.Link("ProductGet", new { id = newProduct.Id });
                return Created(url, model);
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]ProductInputModel model)
        {
            var oldProduct = this.ctx.Products.FirstOrDefault(m => m.Id == id);
            if (oldProduct == null)
            {
                return NotFound();
            }

            oldProduct.Description = model.Description;
            oldProduct.MeasureId = model.MeasureId;
            oldProduct.PricePerUnit = model.PricePerUnit;
            oldProduct.Title = model.Title;

            if (await this.ctx.SaveChangesAsync() > 0)
            {
                return Ok(oldProduct);
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = this.ctx.Products.FirstOrDefault(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            this.ctx.Products.Remove(product);
            if (await this.ctx.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpGet("{id}/photos")]
        public IActionResult GetPhotos(int id)
        {
            var product = this.ctx.Products.Include(p => p.Photos).FirstOrDefault(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var photos = mapper.Map<List<PhotoModel>>(product.Photos.ToList());            
            return Ok(photos);
        }

        [HttpGet("{id}/photos/{photoid}", Name = "PhotoGet")]
        public IActionResult GetPhoto(int id, int photoid)
        {
            var product = this.ctx.Products.Include(p => p.Photos).FirstOrDefault(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var photo = product.Photos.FirstOrDefault(ph => ph.Id == photoid);
            if (photo == null)
            {
                return NotFound();
            }

            return Ok(photo);
        }

        [HttpPost("{id}/photos")]
        public async Task<IActionResult> AddPhoto(int id, [FromBody]PhotoModel model)
        {
            var product = this.ctx.Products.FirstOrDefault(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var newPhoto = mapper.Map<Photo>(model);
            product.Photos.Add(newPhoto);
            if (await this.ctx.SaveChangesAsync() > 0)
            {
                var url = Url.Link("PhotoGet", new { id = id, photoid = newPhoto.Id });
                return Created(url, model);
            }

            return BadRequest();
        }

        [HttpDelete("{id}/photos/{photoid}")]
        public async Task<IActionResult> DeletePhoto(int id, int photoid)
        {
            var product = this.ctx.Products.Include(p => p.Photos).FirstOrDefault(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var photo = product.Photos.FirstOrDefault(ph => ph.Id == photoid);
            if (photo == null)
            {
                return NotFound();
            }

            ctx.Photos.Remove(photo);
            if (await this.ctx.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}