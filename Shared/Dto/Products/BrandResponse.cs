global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;

namespace Shared.Dto.Products
{
    public record BrandResponse
    {
        public int Id { get; init; }
        public string Name { get; init; }

    }
}
