using System.Linq;
using System.Text.Json;
using Domain.Contracts;
using Domain.Identity;
using Domain.Models;
using Domain.Models.Order;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presistance.Data;

namespace Presistance
{
    public class DbInitializer(
        StoreDbContext context,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager) : IDbinitializer
    {
        public async Task InitilizeAsync()
        {
            try
            {
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                if (System.Linq.Enumerable.Any(pendingMigrations))
                    await context.Database.MigrateAsync();

                if (!await roleManager.RoleExistsAsync("Admin"))
                    await roleManager.CreateAsync(new IdentityRole("Admin"));

                if (!await roleManager.RoleExistsAsync("User"))
                    await roleManager.CreateAsync(new IdentityRole("User"));

                if (await userManager.FindByEmailAsync("admin@ecommerce.com") == null)
                {
                    var admin = new AppUser
                    {
                        DisplayName = "Admin",
                        Email = "admin@ecommerce.com",
                        UserName = "admin@ecommerce.com"
                    };
                    var result = await userManager.CreateAsync(admin, "Admin@123");
                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(admin, "Admin");
                }

                if (!context.Set<ProductBrand>().Any())
                {
                    var data = await File.ReadAllTextAsync(@"..\Infrastructure\Presistance\Data\Seeding\brands.json");
                    var objects = JsonSerializer.Deserialize<List<ProductBrand>>(data);
                    if (objects != null && objects.Any())
                    {
                        context.Set<ProductBrand>().AddRange(objects);
                        await context.SaveChangesAsync();
                    }
                }

                if (!context.Set<ProductType>().Any())
                {
                    var data = await File.ReadAllTextAsync(@"..\Infrastructure\Presistance\Data\Seeding\types.json");
                    var objects = JsonSerializer.Deserialize<List<ProductType>>(data);
                    if (objects != null && objects.Any())
                    {
                        context.Set<ProductType>().AddRange(objects);
                        await context.SaveChangesAsync();
                    }
                }

                if (!context.Set<Product>().Any())
                {
                    var data = await File.ReadAllTextAsync(@"..\Infrastructure\Presistance\Data\Seeding\products.json");
                    var objects = JsonSerializer.Deserialize<List<Product>>(data);
                    if (objects != null && objects.Any())
                    {
                        context.Set<Product>().AddRange(objects);
                        await context.SaveChangesAsync();
                    }
                }

                if (!context.Set<DeliveryMethod>().Any())
                {
                    var data = await File.ReadAllTextAsync(@"..\Infrastructure\Presistance\Data\Seeding\delivery.json");
                    var objects = JsonSerializer.Deserialize<List<DeliveryMethod>>(data);
                    if (objects != null && objects.Any())
                    {
                        context.Set<DeliveryMethod>().AddRange(objects);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}