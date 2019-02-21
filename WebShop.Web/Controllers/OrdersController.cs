using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly WebShopContext ctx;
        private readonly IMapper mapper;
        private readonly string userId;

        public OrdersController(WebShopContext ctx, IMapper mapper)
        {
            this.ctx = ctx;
            this.mapper = mapper;
            this.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var orders = mapper.Map<List<OrderModel>>(ctx.Orders.Where(o => o.AppUserId == userId).ToList());
            return Ok(orders);
        }

        [HttpGet("{id}", Name = "OrderGet")]
        public IActionResult Get(int id)
        {
            var order = mapper.Map<OrderModel>(ctx.Orders.FirstOrDefault(o => o.Id == id && o.AppUserId == userId));
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var order = new Order
            {
                AppUserId = userId,
                Date = DateTime.UtcNow,
            };

            ctx.Orders.Add(order);
            var basket = ctx.BasketItems.Include(b => b.Product).Where(i => i.AppUserId == userId);
            foreach (var item in basket)
            {
                ctx.SaleItems.Add(new SaleItem
                {
                    Amount = item.Product.PricePerUnit * item.Units,
                    ProductId = item.ProductId,
                    Units = item.Units,
                    Order = order,
                    Cost = (item.Product.CurrentCost / item.Product.CurrentStock) * item.Units
                });

                ctx.Stock.Add(new StockEntry
                {
                    Date = DateTime.UtcNow,
                    ProductId = item.ProductId,
                    StockEntryType = StockEntryType.Sell,
                    Units = item.Units,
                    Amount = (item.Product.CurrentCost / item.Product.CurrentStock) * item.Units
                });

                ctx.BasketItems.Remove(item);
            }

            if (await this.ctx.SaveChangesAsync() > 0)
            {
                var url = Url.Link("OrderGet", new { id = order.Id });
                return Created(url, order);
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await ctx.Orders.FindAsync(id);
            ctx.Orders.Remove(order);
            var orderItems = this.ctx.SaleItems.Where(i => i.OrderId == id);
            ctx.SaleItems.RemoveRange(orderItems);
            foreach (var item in order.OrderLines)
            {
                ctx.Stock.Add(new StockEntry
                {
                    Amount = item.Cost,
                    ProductId = item.ProductId,
                    StockEntryType = StockEntryType.Increase,
                    Units = item.Units,
                    Date = DateTime.UtcNow
                });
            }

            if (await this.ctx.SaveChangesAsync() > 0)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}