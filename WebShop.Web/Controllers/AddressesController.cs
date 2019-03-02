using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class AddressesController : ControllerBase
    {
        private readonly WebShopContext ctx;
        private readonly IMapper mapper;
        private string userId;

        public AddressesController(WebShopContext ctx, IMapper mapper)
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
            var addresses = mapper.Map<List<AddressModel>>(ctx.Addresses.Where(a => a.CustomerId == UserId).ToList());
            return Ok(addresses);
        }

        [HttpGet("{id}", Name = "AddressGet")]
        public IActionResult Get(int id)
        {
            var address = mapper.Map<AddressModel>(ctx.Addresses.FirstOrDefault(a => a.Id == id && a.CustomerId == UserId));
            return Ok(address);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]AddressInputModel model)
        {
            var entity = mapper.Map<Address>(model);
            entity.CustomerId = UserId;
            this.ctx.Add(entity);
            if (await this.ctx.SaveChangesAsync() > 0)
            {
                var url = Url.Link("AddressGet", new { id = entity.Id });
                return Created(url, mapper.Map<AddressModel>(entity));
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]AddressInputModel model)
        {
            var oldAddress = this.ctx.Addresses.FirstOrDefault(a => a.Id == id && a.CustomerId == UserId);
            if (oldAddress == null)
            {
                return NotFound();
            }

            oldAddress.AddresLine1 = model.AddresLine1;
            oldAddress.AddresLine2 = model.AddresLine2;
            oldAddress.Country = model.Country;
            oldAddress.Postcode = model.Postcode;
            oldAddress.Town = model.Town;

            if (await this.ctx.SaveChangesAsync() > 0)
            {
                return Ok(mapper.Map<AddressModel>(oldAddress));
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var address = this.ctx.Addresses.FirstOrDefault(a => a.Id == id && a.CustomerId == UserId);
            if (address == null)
            {
                return NotFound();
            }

            this.ctx.Addresses.Remove(address);
            if (await this.ctx.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}