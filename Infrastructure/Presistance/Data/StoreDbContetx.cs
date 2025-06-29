global using Domain.Models;
global using Microsoft.EntityFrameworkCore;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;

namespace Presistance.Data
{
    public class StoreDbContetx : DbContext
    {
        public StoreDbContetx(DbContextOptions<StoreDbContetx> options):base(options) { }
          
        public DbSet<Product> products { get; set; }
        public DbSet<ProductType> productType { get; set; }
        public DbSet<ProductBrand> productBrands {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Assembily).Assembly);

        }
    }
}
