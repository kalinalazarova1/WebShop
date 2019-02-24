using System;
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
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly WebShopContext ctx;
        private readonly IMapper mapper;

        public StocksController(WebShopContext ctx, IMapper mapper)
        {
            this.ctx = ctx;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var items = mapper.Map<List<StockEntryModel>>(ctx.Stock.Include(s => s.Product).ToList());
            return Ok(items);
        }

        [HttpGet("{id}", Name = "StockEntryGet")]
        public IActionResult Get(int id)
        {
            var item = mapper.Map<StockEntryModel>(ctx.Stock.Include(s => s.Product).FirstOrDefault(m => m.Id == id));
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]StockEntryInputModel model)
        {
            var entry = mapper.Map<StockEntry>(model);
            entry.Date = DateTime.UtcNow;
            this.ctx.Add(entry);
            UpdateProductStock(model);
            if (await this.ctx.SaveChangesAsync() > 0)
            {
                var url = Url.Link("StockEntryGet", new { id = entry.Id });
                return Created(url, mapper.Map<StockEntryModel>(entry));
            }

            return BadRequest();
        }

        private void UpdateProductStock(StockEntryInputModel stockEntry)
        {
            var product = ctx.Products.Find(stockEntry.ProductId);
            switch (stockEntry.StockEntryType)
            {
                case StockEntryType.Buy:
                case StockEntryType.Increase:
                    product.CurrentStock += stockEntry.Units;
                    product.CurrentCost += stockEntry.Amount;
                    break;
                case StockEntryType.Sell:
                case StockEntryType.Decrease:
                    product.CurrentStock -= stockEntry.Units;
                    product.CurrentCost -= stockEntry.Amount;
                    break;
                default:
                    throw new Exception("Unknown stock entry type");
            }
        }
    }
}