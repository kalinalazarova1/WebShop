﻿using System;
using System.Collections.Generic;
using System.Linq;
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
            var items = mapper.Map<List<StockEntryModel>>(ctx.Stock.ToList());
            return Ok(items);
        }

        [HttpGet("{productid}", Name = "StockEntryGetByProduct")]
        public IActionResult GetByProduct(int productId)
        {
            var items = mapper.Map<StockEntryModel>(ctx.Stock.Where(s => s.ProductId == productId).OrderByDescending(s => s.Date));
            return Ok(items);
        }

        [HttpGet("{id}", Name = "StockEntryGet")]
        public IActionResult Get(int id)
        {
            var item = mapper.Map<StockEntryModel>(ctx.Stock.FirstOrDefault(m => m.Id == id));
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]StockEntryModel model)
        {
            var entry = mapper.Map<StockEntry>(model);
            entry.Date = DateTime.UtcNow;
            this.ctx.Add(entry);
            UpdateProductStock(model);
            if (await this.ctx.SaveChangesAsync() > 0)
            {
                var url = Url.Link("StockEntryGet", new { id = model.Id });
                return Created(url, model);
            }

            return BadRequest();
        }

        private void UpdateProductStock(StockEntryModel stockEntry)
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