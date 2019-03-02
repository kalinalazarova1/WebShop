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
        private string userId;

        public OrdersController(WebShopContext ctx, IMapper mapper)
        {
            this.ctx = ctx;
            this.mapper = mapper;
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
            var orders = mapper.Map<List<OrderModel>>(ctx.Orders
                .Where(o => o.AppUserId == UserId)
                .Include(o => o.Address)
                .Include(o => o.OrderLines)
                .ThenInclude(o => o.Product)
                .ToList());
            return Ok(orders);
        }

        [HttpGet("{id}", Name = "OrderGet")]
        public IActionResult Get(int id)
        {
            var order = mapper.Map<OrderModel>(ctx.Orders
                .Where(o => o.Id == id && o.AppUserId == UserId)
                .Include(o => o.Address)
                .Include(o => o.OrderLines)
                .ThenInclude(o => o.Product)
                .FirstOrDefault());
            return Ok(order);
        }

        [HttpPost("address/{id}")]
        public async Task<IActionResult> Create(int id)
        {
            await ctx.Database.BeginTransactionAsync();
            try
            {
                var order = new Order
                {
                    AppUserId = UserId,
                    Date = DateTime.UtcNow,
                    AddressId = id
                };

                ctx.Orders.Add(order);
                var basket = ctx.BasketItems.Include(b => b.Product).Where(i => i.AppUserId == UserId);
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

                    var product = ctx.Products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product == null)
                    {
                        return BadRequest($"Product unavailable: {item.Product.Title}");
                    }

                    product.CurrentCost -= item.Product.CurrentCost / product.CurrentStock * item.Units;
                    product.CurrentStock -= item.Units;
                    if(product.CurrentStock < 0)
                    {
                        return BadRequest($"Product unavailable: {item.Product.Title}");
                    }

                    ctx.BasketItems.Remove(item);
                    if(await ctx.SaveChangesAsync() == 0)
                    {
                        return BadRequest();
                    }
                }

                var url = Url.Link("OrderGet", new { id = order.Id });
                ctx.Database.CommitTransaction();
                return Created(url, mapper.Map<OrderModel>(order));
            }
            catch (Exception)
            {
                ctx.Database.RollbackTransaction();
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
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
                return NoContent();
            }

            return BadRequest();
        }
    }
}