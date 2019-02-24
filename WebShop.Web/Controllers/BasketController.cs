using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models.Entities;
using WebShop.Web.Filters;
using WebShop.Web.ViewModels;

namespace WebShop.Web.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly WebShopContext ctx;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        private string userId;

        public BasketController(WebShopContext ctx, IMapper mapper, UserManager<AppUser> userManager)
        {
            this.ctx = ctx;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public string UserId
        {
            get
            {
                userId = userId ?? User.FindFirst(ClaimTypes.NameIdentifier).Value;
                return userId;
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            var items = mapper.Map<List<BasketItemModel>>(ctx.BasketItems
                                                             .Where(b => b.AppUserId == UserId)
                                                             .Include(b => b.Product)
                                                             .ToList());
            return Ok(items);
        }

        [HttpGet("{id}", Name = "BasketItemGet")]
        public IActionResult Get(int id)
        {
            var item = mapper.Map<BasketItemModel>(ctx.BasketItems
                                                      .Where(b => b.Id == id && b.AppUserId == UserId)
                                                      .Include(b => b.Product)
                                                      .FirstOrDefault());
            return Ok(item);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody]BasketItemInputModel model)
        {
            var entity = mapper.Map<BasketItem>(model);
            entity.AppUserId = UserId;
            ctx.Add(entity);
            if (await ctx.SaveChangesAsync() > 0)
            {
                var url = Url.Link("BasketItemGet", new { id = entity.Id });
                entity = ctx.BasketItems.Where(i => i.Id == entity.Id).Include(i => i.Product).First();
                return Created(url, mapper.Map<BasketItemModel>(entity));
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Update(int id, [FromBody]BasketItemInputModel model)
        {
            var oldBasketItem = ctx.BasketItems
                .Where(b => b.Id == id && b.AppUserId == UserId)
                .Include(b => b.Product)
                .FirstOrDefault();
            if (oldBasketItem == null)
            {
                return NotFound();
            }

            if(model.Units <= 0)
            {
                return BadRequest("Units should be more than zero.");
            }

            oldBasketItem.Units = model.Units;
            if (await ctx.SaveChangesAsync() > 0)
            {
                return Ok(mapper.Map<BasketItemModel>(oldBasketItem));
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var basketItem = ctx.BasketItems.FirstOrDefault(b => b.Id == id && b.AppUserId == UserId);
            if (basketItem == null)
            {
                return NotFound();
            }

            this.ctx.BasketItems.Remove(basketItem);
            if (await this.ctx.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> ClearBasket()
        {
            var basketItems = this.ctx.BasketItems.Where(b => b.AppUserId == UserId);
            if (basketItems.Count() == 0)
            {
                return NotFound();
            }

            this.ctx.BasketItems.RemoveRange(basketItems);
            var deletedCount = basketItems.Count();
            if (await this.ctx.SaveChangesAsync() == deletedCount)
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}