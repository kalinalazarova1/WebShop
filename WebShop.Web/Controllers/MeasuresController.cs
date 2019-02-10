using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebShop.Data;
using WebShop.Models.Entities;
using WebShop.Web.ViewModels;

namespace WebShop.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class MeasuresController : ControllerBase
    {
        private readonly WebShopContext ctx;
        private readonly IMapper mapper;

        public MeasuresController(WebShopContext ctx, IMapper mapper)
        {
            this.ctx = ctx;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var measures = mapper.Map<List<MeasureModel>>(ctx.Measures.ToList());
            return Ok(measures);
        }

        [HttpGet("{id}", Name = "MeasureGet")]
        public IActionResult Get(int id)
        {
            var measure = mapper.Map<MeasureModel>(ctx.Measures.FirstOrDefault(m => m.Id == id));
            return Ok(measure);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]MeasureModel model)
        {
            this.ctx.Add(mapper.Map<Measure>(model));
            if (await this.ctx.SaveChangesAsync() > 0)
            {
                var url = Url.Link("MeasureGet", new { id = model.Id });
                return Created(url, model);
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]MeasureModel model)
        {
            var oldMeasure = this.ctx.Measures.FirstOrDefault(m => m.Id == id);
            if (oldMeasure == null)
            {
                return NotFound();
            }

            oldMeasure.Symbol = model.Symbol;
            if (await this.ctx.SaveChangesAsync() > 0)
            {
                return Ok(oldMeasure);
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var measure = this.ctx.Measures.FirstOrDefault(m => m.Id == id);
            if (measure == null)
            {
                return NotFound();
            }

            this.ctx.Measures.Remove(measure);
            if (await this.ctx.SaveChangesAsync() > 0)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}