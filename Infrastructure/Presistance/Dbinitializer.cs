global using Domain.Contracts;
global using Presistance.Data;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Text.Json;
global using System.Threading.Tasks;

namespace Presistance
{
    public class Dbinitializer(StoreDbContetx context) 
        : IDbinitializer
    {
        public async Task InitilizeAsync()
        {
            try {
                //if ((await context.Database.GetPendingMigrationsAsync()).Any())
                //    await context.Database.MigrateAsync();

                if (!context.Set<ProductBrand>().Any())
                {
                    //read data from json file
                    //conver to [Deserlize]
                    //add to context
                    //await context.SaveChangesAsync();

                    var data = await File.ReadAllTextAsync(@"..\Infrastructure\Presistance\Data\Seeding\brands.json");
                    var objects = JsonSerializer.Deserialize<List<ProductBrand>>(data);
                    //save Db
                    if (objects != null && objects.Any())
                    {
                        context.Set<ProductBrand>().AddRange(objects);
                        await context.SaveChangesAsync();
                    }
                }


                if (!context.Set<ProductType>().Any())
                {
                    //read data from json file
                    //conver to [Deserlize]
                    //add to context
                    //await context.SaveChangesAsync();

                    var data = await File.ReadAllTextAsync(@"..\Infrastructure\Presistance\Data\Seeding\types.json");
                    var objects = JsonSerializer.Deserialize<List<ProductType>>(data);
                    //save Db
                    if (objects != null && objects.Any())
                    {
                        context.Set<ProductType>().AddRange(objects);
                        await context.SaveChangesAsync();
                    }
                }


                if (!context.Set<Product>().Any())
                {
                    //read data from json file
                    //conver to [Deserlize]
                    //add to context
                    //await context.SaveChangesAsync();

                    var data = await File.ReadAllTextAsync(@"..\Infrastructure\Presistance\Data\Seeding\products.json");
                    var objects = JsonSerializer.Deserialize<List<Product>>(data);
                    //save Db
                    if (objects != null && objects.Any())
                    {
                        context.Set<Product>().AddRange(objects);
                        await context.SaveChangesAsync();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
