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

        [HttpGet("{id}", Name = "StockEntryGet")]
        public IActionResult Get(int id)
        {
            var item = mapper.Map<StockEntryModel>(ctx.Stock.FirstOrDefault(m => m.Id == id));
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]StockEntryModel model)
        {
            this.ctx.Add(mapper.Map<StockEntry>(model));
            if (await this.ctx.SaveChangesAsync() > 0)
            {
                var url = Url.Link("StockEntryGet", new { id = model.Id });
                return Created(url, model);
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]StockEntryModel model)
        {
            var oldStockEntry = this.ctx.Stock.FirstOrDefault(m => m.Id == id);
            if (oldStockEntry == null)
            {
                return NotFound();
            }

            oldStockEntry.Amount = model.Amount;
            oldStockEntry.Date = model.Date;
            oldStockEntry.ProductId = model.ProductId;
            oldStockEntry.StockEntryType = model.StockEntryType;
            oldStockEntry.Units = model.Units;
            if (await this.ctx.SaveChangesAsync() > 0)
            {
                return Ok(oldStockEntry);
            }

            return BadRequest();
        }
    }
}