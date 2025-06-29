global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;

namespace Shared.Dto.Products
{
    public record ProductsRespone
    {


        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public string PictureUrl { get; init; }
        public string BrandName { get; init; }
        public string TyepeName { get; init; }




    }
}
