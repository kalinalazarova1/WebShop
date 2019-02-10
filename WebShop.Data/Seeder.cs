using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebShop.Models.Entities;

namespace WebShop.Data
{
    public class Seeder
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<AppUser> userManager;
        private WebShopContext ctx;
        private Guid[] userIds;

        public Seeder(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, WebShopContext ctx)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.ctx = ctx;
            this.userIds = new Guid[] { Guid.NewGuid(), Guid.NewGuid() };
        }

        public async Task Seed()
        {
            await SeedUser("kalina.lazarova@gmail.com", 0);
            await SeedUser("peter.ivanov@gmail.com", 1);
            await SeedProducts();
        }

        private async Task SeedUser(string email, int index)
        {
            var user = await userManager.FindByNameAsync(email);

            if (user == null)
            {
                if (!(await roleManager.RoleExistsAsync("Admin")))
                {
                    var role = new IdentityRole("Admin");
                    await roleManager.CreateAsync(role);
                }

                user = new AppUser()
                {
                    UserName = email,
                    Email = email,
                    Id = this.userIds[index].ToString()
                };

                var userResult = await userManager.CreateAsync(user, "Kalina1@");
                var roleResult = await userManager.AddToRoleAsync(user, "Admin");

                if (!userResult.Succeeded || !roleResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }
            }
        }

        private async Task SeedProducts()
        {
            if (!ctx.Measures.Any())
            {
                ctx.AddRange(new List<Measure>
                {
                    new Measure { Symbol = "pcs" },
                    new Measure { Symbol = "kg" },
                    new Measure { Symbol = "g" },
                    new Measure { Symbol = "m" },
                    new Measure { Symbol = "cm" }
                });
            }

            if (!ctx.Products.Any())
            {
                var measureId = ctx.Measures.First(m => m.Symbol == "pcs").Id;
                ctx.AddRange(new List<Product>
                {
                    new Product { Title = "Socks", MeasureId = measureId, PricePerUnit = 2m },
                    new Product { Title = "Pants", MeasureId = measureId, PricePerUnit = 5m },
                    new Product { Title = "Vests", MeasureId = measureId, PricePerUnit = 3m }
                });

                await ctx.SaveChangesAsync();
            }
        }
    }
}
