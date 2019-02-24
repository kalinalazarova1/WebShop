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
        private Guid userId;

        public Seeder(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, WebShopContext ctx)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.ctx = ctx;
            this.userId = Guid.NewGuid();
        }

        public async Task Seed()
        {
            await SeedRoles();
            await SeedUser("kalina.lazarova@gmail.com", 0);
            await SeedProducts();
        }

        private async Task SeedRoles()
        {
            if (!(await roleManager.RoleExistsAsync("Admin")))
            {
                var role = new IdentityRole("Admin");
                await roleManager.CreateAsync(role);
            }

            if (!(await roleManager.RoleExistsAsync("User")))
            {
                var role = new IdentityRole("User");
                await roleManager.CreateAsync(role);
            }
        }

        private async Task SeedUser(string email, int index)
        {
            var user = await userManager.FindByNameAsync(email);

            if (user == null)
            {
                user = new AppUser()
                {
                    UserName = email,
                    Email = email,
                    Id = userId.ToString()
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

                await ctx.SaveChangesAsync();
            }

            if (!ctx.Categories.Any())
            {
                ctx.AddRange(new List<Category>
                {
                    new Category { Title = "General" }
                });

                await ctx.SaveChangesAsync();
            }

            if (!ctx.Products.Any())
            {
                var measureId = ctx.Measures.First(m => m.Symbol == "pcs").Id;
                var categoryId = ctx.Categories.First().Id;
                ctx.AddRange(new List<Product>
                {
                    new Product { Title = "Socks", MeasureId = measureId, PricePerUnit = 2m, CategoryId = categoryId },
                    new Product { Title = "Pants", MeasureId = measureId, PricePerUnit = 5m, CategoryId = categoryId },
                    new Product { Title = "Vests", MeasureId = measureId, PricePerUnit = 3m, CategoryId = categoryId }
                });

                await ctx.SaveChangesAsync();
            }
        }
    }
}
