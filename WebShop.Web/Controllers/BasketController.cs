using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebShop.Data;
using WebShop.Models.Entities;
using WebShop.Web.ViewModels;

namespace WebShop.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly WebShopContext ctx;
        private readonly IMapper mapper;
        private readonly string userId;

        public BasketController(WebShopContext ctx, IMapper mapper)
        {
            this.ctx = ctx;
            this.mapper = mapper;
            this.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var items = mapper.Map<List<BasketItemModel>>(ctx.BasketItems
                                                             .Where(b => b.AppUserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                                                             .ToList());
            return Ok(items);
        }

        [HttpGet("{id}", Name = "BasketItemGet")]
        public IActionResult Get(int id)
        {
            var item = mapper.Map<BasketItemModel>(ctx.BasketItems
                                                      .FirstOrDefault(b => b.Id == id && b.AppUserId == userId));
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]BasketItemModel model)
        {
            this.ctx.Add(mapper.Map<BasketItem>(model));
            model.AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (await this.ctx.SaveChangesAsync() > 0)
            {
                var url = Url.Link("BasketItemGet", new { id = model.Id });
                return Created(url, model);
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]BasketItemModel model)
        {
            var oldBasketItem = this.ctx.BasketItems.FirstOrDefault(b => b.Id == id);
            if (oldBasketItem == null)
            {
                return NotFound();
            }

            oldBasketItem.Amount = model.Amount;
            oldBasketItem.ProductId = model.ProductId;
            oldBasketItem.Units = model.Units;
            if (await this.ctx.SaveChangesAsync() > 0)
            {
                return Ok(oldBasketItem);
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var basketItem = this.ctx.BasketItems.FirstOrDefault(b => b.Id == id && b.AppUserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (basketItem == null)
            {
                return NotFound();
            }

            this.ctx.BasketItems.Remove(basketItem);
            if (await this.ctx.SaveChangesAsync() > 0)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> ClearBasket()
        {
            var basketItems = this.ctx.BasketItems.Where(b => b.AppUserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (basketItems.Count() == 0)
            {
                return NotFound();
            }

            this.ctx.BasketItems.RemoveRange(basketItems);
            if (await this.ctx.SaveChangesAsync() == basketItems.Count())
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}