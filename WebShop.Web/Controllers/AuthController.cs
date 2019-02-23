using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebShop.Data;
using WebShop.Models.Entities;
using WebShop.Web.Filters;
using WebShop.Web.ViewModels;

namespace WebShop.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IPasswordHasher<AppUser> hasher;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;
        private readonly WebShopContext ctx;

        public AuthController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> hasher, IConfiguration configuration, IMapper mapper, WebShopContext ctx)
        {
            this.hasher = hasher;
            this.userManager = userManager;
            this.configuration = configuration;
            this.mapper = mapper;
            this.ctx = ctx;
        }

        [AllowAnonymous]
        [HttpPost("token")]
        [ValidateModel]
        public async Task<IActionResult> CreateToken([FromBody] CredentialsModel model)
        {
            try
            {
                var user = await this.userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(JwtRegisteredClaimNames.Sub, model.Username)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["Tokens:Key"]));
                    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                    if (this.hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
                    {
                        var token = new JwtSecurityToken(
                        issuer: this.configuration["Tokens:Issuer"],
                        audience: this.configuration["Tokens:Issuer"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddHours(2),
                        signingCredentials: credentials);

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                }
            }
            catch (Exception)
            {
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ValidateModel]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                user = new AppUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Surname = model.Surname
                };

                var userResult = await userManager.CreateAsync(user, model.Password);
                if (!userResult.Succeeded)
                {
                    return BadRequest(userResult.Errors);
                }

                var roleResult = await userManager.AddToRoleAsync(user, "User");
                if (!roleResult.Succeeded)
                {
                    return BadRequest(roleResult.Errors);
                }

                var newUser = ctx.AppUsers.Where(u => u.Id == user.Id).Include(u => u.Orders).Include(u => u.Basket).First();
                return Ok(mapper.Map<AppUserModel>(newUser));
            }

            return BadRequest("User with that email is already registered.");
        }

        // confirm email

        // forgot password
    }
}